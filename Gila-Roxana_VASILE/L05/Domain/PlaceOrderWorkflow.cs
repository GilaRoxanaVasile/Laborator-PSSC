using System;
using Domain.Models;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using L02_PSSC.Domain;
using static Domain.Models.OrderPlacedEvent;
using static Domain.CartOperation;
using static L02_PSSC.Domain.Cart;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using Domain.Repositories;

namespace Domain.Models
{
    public class PlaceOrderWorkflow
    {
        private readonly ILogger<PlaceOrderWorkflow> logger;
        private readonly IOrderLineRepository lineRepository;
        private readonly IOrderHeaderRepository headerRepository;
        private readonly IProductRepository productRepository;

        public PlaceOrderWorkflow(ILogger<PlaceOrderWorkflow> logger, IOrderLineRepository lineRepository, IOrderHeaderRepository headerRepository, IProductRepository productRepository)
        {
            this.logger = logger;
            this.lineRepository = lineRepository;
            this.headerRepository = headerRepository;
            this.productRepository = productRepository;
        }

        public async Task <IOrderPlacedEvent> ExecuteAsync(Client client, Guid cartID, PlaceOrderCommand command, Func<ProductCode,TryAsync<bool>> checkProductExists)
        {
            UnvalidatedCart unvalidatedClientCart = new UnvalidatedCart(cartID, command.InputProducts);
            
            ICart cart = await ValidateCartProducts(client, cartID, checkProductExists, unvalidatedClientCart);
            cart = CalculateFinalPrices(cart);
            cart = SendOrder(cart);

            return cart.Match(
                whenEmptyCart: emptyCart => new OrderPlacingFailedEvent("invalid") as IOrderPlacedEvent,
                whenInvalidatedCart: invalidCart => new OrderPlacingFailedEvent(invalidCart.reason),
                whenValidatedCart: validatedCart => new OrderPlacingFailedEvent("invalid"),
                whenUnvalidatedCart: unvalidatedCart => new OrderPlacingFailedEvent("invalid"),
                whenCalculatedCartPrice: calculatedCart => new OrderPlacingFailedEvent("invalid"),
                whenPayedCart: payedCart => new OrderPlacingSucceded(payedCart.Csv, DateTime.Now)
                );
        }
        
        private Option<ProductCode> CheckProductExists(IEnumerable<ProductCode> products, ProductCode product)
        {
            if(products.Any(p=>p==product))
            {
                return Some(product);
            }
            else
            {
                return None;
            }
        }

        /*
        private async Task<Either<ICart, PayedCart>> ExecuteWorkflowAsync(Client client, Guid cartID, UnvalidatedCart unvalidated, IEnumerable<CalculatedOrder> existingProducts, Func<ProductCode, Option<ProductCode>> checkProductExists)
        {
            ICart productList = await ValidateCartProducts(client, cartID, checkProductExists, unvalidated);
        }
        */

        /*
        private OrderPlacingFailedEvent GenerateFailedEvent(ICart cart) =>
            cart.Match<OrderPlacingFailedEvent>(
                whenUnvalidatedCart: unvalidatedCart => new($"Invalid {nameof(UnvalidatedCart)}"),
                whenInvalidatedCart: invalidatedCart => new($"Invalid {nameof(InvalidatedCart)}"),
                whenValidatedCart: validatedCart => new($"Invalid {nameof(UnvalidatedCart)}"),
                whenFailedCart: failedCart => 
                    {
                                                
                                               
                    }
                whenCalculatedCart: calculatedCart => new($"Invalid {nameof(UnvalidatedCart)}"),
                whenPayedCart: payedCart => new($"Invalid {nameof(UnvalidatedCart)}")
                );
        */

    }
}
