using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageExt.Prelude;
using LanguageExt;
using ProiectPSSC.Domain.Models;
using static ProiectPSSC.Domain.Models.OrderProducts;
using System.Data;
using Microsoft.Extensions.Logging;
using ProiectPSSC.Domain.Repositories;
using static ProiectPSSC.Domain.Models.OrderPlacedEvent;
using static ProiectPSSC.Domain.OrderProductsOperation;

namespace ProiectPSSC.Domain
{
    public class PlaceOrderWorkflow
    {
        private readonly ILogger<PlaceOrderWorkflow> logger;
        private readonly IProductRepository productRepository;
        private readonly IOrderLineRepository orderLineRepository;
        private readonly IOrderHeaderRepository orderHeaderRepository;
        private readonly IClientRepository clientRepository;

        public PlaceOrderWorkflow(ILogger<PlaceOrderWorkflow> logger, IProductRepository productRepository, IOrderLineRepository orderLineRepository, IOrderHeaderRepository orderHeaderRepository, IClientRepository clientRepository)
        {
            this.logger = logger;
            this.productRepository = productRepository;
            this.orderLineRepository = orderLineRepository;
            this.orderHeaderRepository = orderHeaderRepository;
            this.clientRepository = clientRepository;
        }

        public async Task<IOrderPlacedEvent> EventAsync(PlaceOrderCommand command)
        {
            UnvalidatedOrderProducts unvalidatedOrder = new UnvalidatedOrderProducts(command.InputClientProducts);

            var result = from products in productRepository.TryGetExistingProducts(unvalidatedOrder.ProductList.Select(product => product.ProductCode))
                                                   .ToEither(ex => new InvalidOrderProducts(unvalidatedOrder.ProductList, "eroare la product") as IOrderProducts)
                         let checkProductExists = (Func<ProductCode, Option<ProductCode>>)(product => CheckProducttExists(products, product))

                         from productStoc in productRepository.TryGetProductStoc(unvalidatedOrder.ProductList.Select(product => product.ProductCode))
                                                   .ToEither(ex => new InvalidOrderProducts(unvalidatedOrder.ProductList, "eroare la product stoc") as IOrderProducts)
                         let checkStocAvailable = (Func<Quantity, Option<Quantity>>)(product => CheckStocAvailable(productStoc, product))

                         from productPrices in productRepository.TryGetProductPrices(unvalidatedOrder.ProductList.Select(product => product.ProductCode))
                                                  .ToEither(ex => new InvalidOrderProducts(unvalidatedOrder.ProductList, "eroare la product price") as IOrderProducts)

                         from productCatalog in productRepository.TryGetProductCatalog(unvalidatedOrder.ProductList.Select(product => product.ProductCode))
                                                  .ToEither(ex => new InvalidOrderProducts(unvalidatedOrder.ProductList, "eroare la product catalog") as IOrderProducts)

                        // from allOrderProducts in orderLineRepository.TryGetExistingOrderProducts()
                          //                       .ToEither(ex => new InvalidOrderProducts(unvalidatedOrder.ProductList, "eroare la produs") as IOrderProducts)

                         from existingClients in clientRepository.TryGetExistingClients(unvalidatedOrder.ProductList.Select(client => client.ClientEmail))
                                                 .ToEither(ex => new InvalidOrderProducts(unvalidatedOrder.ProductList, "eroare la client") as IOrderProducts)
                         let checkClientExists = (Func<ClientEmail, Option<ClientEmail>>)(client => CheckClientExists(existingClients, client))

                         from clientOrders in orderHeaderRepository.TryGetExistingClientOrders()
                                                 .ToEither(ex => new InvalidOrderProducts(unvalidatedOrder.ProductList, "eroare la client orders") as IOrderProducts)

                         from placedOrder in ExecuteWorkflowAsync(unvalidatedOrder, clientOrders, productCatalog, checkClientExists, checkStocAvailable, checkProductExists)
                                                 .ToAsync()

                         from _ in orderHeaderRepository.TrySaveOrders(placedOrder)
                                                 .ToEither(ex => new InvalidOrderProducts(unvalidatedOrder.ProductList, "eroare la order header") as IOrderProducts)

                        // from __ in orderLineRepository.TrySaveProducts(placedOrder)
                          //                      .ToEither(ex => new InvalidOrderProducts(unvalidatedOrder.ProductList, "eroare la order line") as IOrderProducts)
                         
                         select placedOrder;

            return await result.Match(
                Left: order => GenerateFailedEvent(order) as IOrderPlacedEvent,
                Right: placedOrder => new OrderPlacedSuccededEvent(placedOrder.ClientEmail, placedOrder.Price, placedOrder.Csv, placedOrder.PublishedDate)
                );

         }
        private async Task<Either<IOrderProducts, PlacedOrderProducts>> ExecuteWorkflowAsync
        (UnvalidatedOrderProducts unvalidatedOrder, IEnumerable<CalculatedProductPrice> existingOrders, IEnumerable<Products> productCatalog,
            Func<ClientEmail, Option<ClientEmail>> checkClientExists, Func<Quantity, Option<Quantity>> checkStocAvailable, Func <ProductCode, Option<ProductCode>> checkProductExists)
        {
            IOrderProducts order = await ValidateOrder2(checkClientExists, checkProductExists, checkStocAvailable, unvalidatedOrder);
            order = CalculateFinalPrices(order, productCatalog);
            order = MergeProducts(order, existingOrders);
            order = PlaceOrder(order); 

            return order.Match<Either<IOrderProducts, PlacedOrderProducts>>(
                whenUnvalidatedOrderProducts: unvalidatedClientOrder => Left(unvalidatedClientOrder as IOrderProducts),
                whenInvalidOrderProducts: invalidatedClientOrder => Left(invalidatedClientOrder as IOrderProducts),
                whenValidatedOrderProducts: validatedOrder => Left(validatedOrder as IOrderProducts),
                whenCalculatedOrderProducts: calculatedOrderProducts => Left(calculatedOrderProducts as IOrderProducts),
                whenPlacedOrderProducts: placedOrder => Right(placedOrder)
                    );
        }
        private Option<ClientEmail> CheckClientExists(IEnumerable<ClientEmail> clients, ClientEmail client)
        {
            if(clients.Any(c => c == client))
            {
                return Some(client);
            }
            else
            {
                return None;
            }
        }


        private Option<Quantity> CheckStocAvailable(IEnumerable<Quantity> stoc, Quantity quantity)
        {
            if (stoc.Any(c => c.Value > quantity.Value))
            {
                return Some(quantity);
            }
            else
            {
                return None;
            }
        }

        private Option<ProductCode> CheckProducttExists(IEnumerable<ProductCode> products, ProductCode product)
        {
            if (products.Any(p => p == product))
            {
                return Some(product);
            }
            else
            {
                return None;
            }
        }

        private OrderPlacedFailedEvent GenerateFailedEvent(IOrderProducts order) =>
            order.Match<OrderPlacedFailedEvent>(
                whenUnvalidatedOrderProducts: unvalidatedClientOrder => new($"Invalid state {nameof(UnvalidatedOrderProducts)}"),
                whenInvalidOrderProducts: invalidatedClientOrder => new(invalidatedClientOrder.Reason),
                whenValidatedOrderProducts: validatedOrder => new($"Invalid state {nameof(ValidatedOrderProducts)}"),
                whenCalculatedOrderProducts: calculatedOrderProducts => new($"Invalid state {nameof(CalculatedOrderProducts)}"),
                whenPlacedOrderProducts: placedOrder => new($"Invalid state {nameof(PlacedOrderProducts)}"));
                
    }
}