using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageExt.Prelude;
using LanguageExt;
using ProiectPSSC.Domain.Models;
using static ProiectPSSC.Domain.Models.OrderProducts;
using ProiectPSSC.Domain.Models;
using System.Data;

namespace ProiectPSSC.Domain
{
    public static class OrderProductsOperation
    {

        public static Task<IOrderProducts> ValidateOrder2(Func<ClientEmail, Option<ClientEmail>> checkClientExists, Func<ProductCode, Option<ProductCode>> checkProductExists,
                                                                Func<Quantity, Option<Quantity>> checkStocAvailable, UnvalidatedOrderProducts orderProducts) =>
            orderProducts.ProductList
                        .Select(ValidateOrderClients2(checkClientExists, checkStocAvailable, checkProductExists))
                        .Aggregate(CrateEmptyValidatedOrderProductsList().ToAsync(), ReduceValidProducts)
                        .MatchAsync(
                            Right: validatedOrderProducts => new ValidatedOrderProducts(validatedOrderProducts),
                            LeftAsync: errorMessage => Task.FromResult((IOrderProducts)new InvalidOrderProducts(orderProducts.ProductList, errorMessage))
                        );

        private static Func<UnvalidatedClientOrder, EitherAsync<string, ValidatedClientOrder>> ValidateOrderClients2(Func<ClientEmail, Option<ClientEmail>> checkClientExists,
                                                Func<Quantity, Option<Quantity>> checkStocAvailable, Func<ProductCode, Option<ProductCode>> checkProductExists) =>
           unvalidatedClientProducts => ValidateOrderClients2(checkClientExists, checkStocAvailable, checkProductExists, unvalidatedClientProducts);

        private static EitherAsync<string, ValidatedClientOrder> ValidateOrderClients2(Func<ClientEmail, Option<ClientEmail>> checkClientExists,
                        Func<Quantity, Option<Quantity>> checkStocAvailable, Func<ProductCode, Option<ProductCode>> checkProductExists, UnvalidatedClientOrder unvalidatedClientOrder) =>
            from productCode in ProductCode.TryParseProductCode(unvalidatedClientOrder.ProductCode)
                                            .ToEitherAsync($"Invalid product code ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.ProductCode})")
            from productExists in checkProductExists(productCode)
                     .ToEitherAsync($"Product {productCode.Value} does not exist.")

            from quantity in Quantity.TryParseQuantity(unvalidatedClientOrder.Quantity)
                                      .ToEitherAsync($"Invalid quantity ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.Quantity})")
            from stocAvailable in checkStocAvailable(quantity)
                                .ToEitherAsync($"Quantity for product {productCode.Value} is too much.")

            from clientEmail in ClientEmail.TryParseClientEmail(unvalidatedClientOrder.ClientEmail)
                                .ToEitherAsync($"Invalid client email ({unvalidatedClientOrder.ClientEmail})")
            from price in ProductPrice.TryParsePrice(unvalidatedClientOrder.productPrice)
                            .ToEitherAsync($"Invalid product price ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.ProductCode})")
            from clientExists in checkClientExists(clientEmail)
                                 .ToEitherAsync($"Client {clientEmail.Value} does not exist.")
            select new ValidatedClientOrder(clientEmail, productCode, quantity, price);

        /*
        //validare existenta client
        public static Task<IOrderProducts> ValidateOrder(Func<ClientEmail, Option<ClientEmail>> checkClientExists, UnvalidatedOrderProducts orderProducts) =>
            orderProducts.ProductList
                        .Select(ValidateOrderClients(checkClientExists))
                        .Aggregate(CrateEmptyValidatedOrderProductsList().ToAsync(), ReduceValidProducts)
                        .MatchAsync(
                            Right: validatedOrderProducts => new ValidatedOrderProducts(validatedOrderProducts),
                            LeftAsync: errorMessage => Task.FromResult((IOrderProducts) new InvalidOrderProducts(orderProducts.ProductList, errorMessage))
                        );
        private static Func<UnvalidatedClientOrder, EitherAsync<string, ValidatedClientOrder>> ValidateOrderClients(Func<ClientEmail, Option<ClientEmail>> checkClientExists) =>
            unvalidatedClientProducts => ValidateOrderClients(checkClientExists, unvalidatedClientProducts);

        private static EitherAsync<string, ValidatedClientOrder> ValidateOrderClients(Func<ClientEmail, Option<ClientEmail>> checkClientExists, UnvalidatedClientOrder unvalidatedClientOrder) =>
            from orderProductCode in ProductCode.TryParseProductCode(unvalidatedClientOrder.ProductCode)
                                            .ToEitherAsync($"Invalid product code ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.ProductCode})")
            from quantity in Quantity.TryParseQuantity(unvalidatedClientOrder.Quantity)
                                      .ToEitherAsync($"Invalid quantity ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.Quantity})")
            from clientEmail in ClientEmail.TryParseClientEmail(unvalidatedClientOrder.ClientEmail)
                                .ToEitherAsync($"Invalid client email ({unvalidatedClientOrder.ClientEmail})")
            from price in ProductPrice.TryParsePrice(unvalidatedClientOrder.productPrice)
                            .ToEitherAsync($"Invalid product price ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.ProductCode})")
            from clientExists in checkClientExists(clientEmail)
                                 .ToEitherAsync($"Client {clientEmail.Value} does not exist.")
            select new ValidatedClientOrder(clientEmail, orderProductCode, quantity, price);



        //validare existenta produs
        public static Task<IOrderProducts> ValidateProduct(Func<ProductCode, Option<ProductCode>> checkProductExists, UnvalidatedOrderProducts orderProducts) =>
        orderProducts.ProductList
                .Select(ValidateOrderProducts(checkProductExists))
                .Aggregate(CrateEmptyValidatedOrderProductsList().ToAsync(), ReduceValidProducts)
                .MatchAsync(
                    Right: validatedOrderProducts => new ValidatedOrderProducts(validatedOrderProducts),
                    LeftAsync: errorMessage => Task.FromResult((IOrderProducts)new InvalidOrderProducts(orderProducts.ProductList, errorMessage))
                );
        private static Func<UnvalidatedClientOrder, EitherAsync<string, ValidatedClientOrder>> ValidateOrderProducts(Func<ProductCode, Option<ProductCode>> checkProductExists) =>
            unvalidatedClientProducts => ValidateOrderProducts(checkProductExists, unvalidatedClientProducts);

        private static EitherAsync<string, ValidatedClientOrder> ValidateOrderProducts(Func<ProductCode, Option<ProductCode>> checkProductExists, UnvalidatedClientOrder unvalidatedClientOrder) =>
            from productCode in ProductCode.TryParseProductCode(unvalidatedClientOrder.ProductCode)
                                            .ToEitherAsync($"Invalid product code ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.ProductCode})")
            from quantity in Quantity.TryParseQuantity(unvalidatedClientOrder.Quantity)
                                      .ToEitherAsync($"Invalid quantity ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.Quantity})")
            from clientEmail in ClientEmail.TryParseClientEmail(unvalidatedClientOrder.ClientEmail)
                                .ToEitherAsync($"Invalid client email ({unvalidatedClientOrder.ClientEmail})")
            from price in ProductPrice.TryParsePrice(unvalidatedClientOrder.productPrice)
                            .ToEitherAsync($"Invalid product code ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.ProductCode})")
            from productExists in checkProductExists(productCode)
                                 .ToEitherAsync($"Product {productCode.Value} does not exist.")
            select new ValidatedClientOrder(clientEmail, productCode, quantity, price);
        */

        
        //creare lista order goala
        private static Either<string, List<ValidatedClientOrder>> CrateEmptyValidatedOrderProductsList() =>
            Right(new List<ValidatedClientOrder>());

        private static EitherAsync<string, List<ValidatedClientOrder>> ReduceValidProducts(EitherAsync<string, List<ValidatedClientOrder>> acc, EitherAsync<string, ValidatedClientOrder> next) =>
            from list in acc
            from nextProduct in next
            select list.AppendValidProduct(nextProduct);

        private static List<ValidatedClientOrder> AppendValidProduct(this List<ValidatedClientOrder> list, ValidatedClientOrder validProduct)
        {
            list.Add(validProduct);
            return list;
        }

        public static IOrderProducts CalculateFinalOrdersPrices(IOrderProducts orderProducts) => orderProducts.Match(
           whenUnvalidatedOrderProducts: unvalidatedClientOrder => unvalidatedClientOrder,
           whenInvalidOrderProducts: invalidatedClientOrder => invalidatedClientOrder,
           whenPlacedOrderProducts: placedOrder => placedOrder,
           whenCalculatedOrderProducts: calculatedOrderProducts => calculatedOrderProducts,
           whenValidatedOrderProducts: CalculateOrderFinalPrice
       );

        public static IOrderProducts CalculateFinalPrices(IOrderProducts orderProducts) => orderProducts.Match(
            whenUnvalidatedOrderProducts: unvalidatedClientOrder => unvalidatedClientOrder,
            whenInvalidOrderProducts: invalidatedClientOrder => invalidatedClientOrder,
            whenPlacedOrderProducts: placedOrder => placedOrder,
            whenCalculatedOrderProducts: calculatedOrderProducts => calculatedOrderProducts,
            whenValidatedOrderProducts: CalculateProductFinalPrice
            );

        private static IOrderProducts CalculateProductFinalPrice(ValidatedOrderProducts validOrder) =>
            new CalculatedOrderProducts(validOrder.ProductList
                                                   .Select(CalculateFinalProductPrice)
                                                   .ToList()
                                                   .AsReadOnly());
        private static IOrderProducts CalculateOrderFinalPrice(ValidatedOrderProducts validOrder) =>
         new CalculatedOrderProducts(validOrder.ProductList
                                           .Select(CalculateFinalProductPrice)
                                           .ToList()
                                           .AsReadOnly());

        /*
        public static CalculatedOrderTotalPayment CalculateFinalOrderPayment(ValidatedClientOrder validatedClientOrder, CalculatedProductPrice calculatedProduct, decimal oldTotalOrderPrice)
        { 
            return new CalculatedOrderTotalPayment(validatedClientOrder.clientEmail, calculatedProduct, new ProductPrice(calculatedProduct.price.Price + oldTotalOrderPrice));
            
           return new CalculatedOrderTotalPayment
                 (validatedClientOrder.clientEmail,
                 new CalculatedProductPrice(validatedClientOrder.productCode, validatedClientOrder.quantity, validatedClientOrder.price, new ProductPrice(validatedClientOrder.price.Price * validatedClientOrder.quantity.Value)),
                 new ProductPrice(validatedClientOrder.price.Price * validatedClientOrder.quantity.Value)); 
        }
        */

        private static CalculatedProductPrice CalculateFinalProductPrice2(ValidatedClientOrder validatedClientOrder, IEnumerable<Products> catalog)
        {
            var productPrice= catalog.Where(c => validatedClientOrder.productCode == c.code)
                                .Select(c => c.price);

            return new CalculatedProductPrice(validatedClientOrder.productCode,
        validatedClientOrder.quantity, validatedClientOrder.price, new ProductPrice(validatedClientOrder.price.Price * validatedClientOrder.quantity.Value));
        }

        //calculez pret total produs
        private static CalculatedProductPrice CalculateFinalProductPrice(ValidatedClientOrder validatedClientOrder) 
             => new CalculatedProductPrice(validatedClientOrder.productCode,
                validatedClientOrder.quantity, validatedClientOrder.price, new ProductPrice(validatedClientOrder.price.Price * validatedClientOrder.quantity.Value)); 

        public static IOrderProducts MergeProducts(IOrderProducts products, IEnumerable<CalculatedProductPrice> existingProducts) =>
            products.Match(
            whenUnvalidatedOrderProducts: unvalidatedClientOrder => unvalidatedClientOrder,
            whenInvalidOrderProducts: invalidatedClientOrder => invalidatedClientOrder,
            whenPlacedOrderProducts: placedOrder => placedOrder,
            whenValidatedOrderProducts: validatedOrder => validatedOrder,
            whenCalculatedOrderProducts: calculatedOrderProducts => MergeProducts(calculatedOrderProducts.ProductList, existingProducts)
                );

        private static CalculatedOrderProducts MergeProducts(IEnumerable<CalculatedProductPrice> newList, IEnumerable<CalculatedProductPrice> existingList)
        {
            var updatedAndNewProducts = newList.Select(product => product with { ProductId = existingList.FirstOrDefault(g => g.code == product.code)?.ProductId ?? 0, IsUpdated = true });
            var oldProducts = existingList.Where(product => !newList.Any(g => g.code == product.code));
            var allProducts = updatedAndNewProducts.Union(oldProducts)
                                               .ToList()
                                               .AsReadOnly();
            return new CalculatedOrderProducts(allProducts);
        }

        /*
        private static IOrderProducts MergeOrders(IEnumerable<CalculatedOrderTotalPayment> newList, IEnumerable<CalculatedOrderTotalPayment> existingList)
        {
            var updatedAndNewOrders = newList.Select
                        (order => order with { OrderId = existingList.FirstOrDefault(g => g.clientEmail == order.clientEmail)?.OrderId ?? 0, IsUpdated = true });
            var oldOrders = existingList.Where(order => !newList.Any(g => g.clientEmail == order.clientEmail));
            var allOrders = updatedAndNewOrders.Union(oldOrders)
                                               .ToList()
                                               .AsReadOnly();
            return new PlacedOrderProducts(allOrders, newList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(), DateTime.Now);
        }
        */


        /*
        private static IOrderProducts MergeOrder(IEnumerable<CalculatedOrderTotalPayment> newList, IEnumerable<CalculatedOrderTotalPayment> existingList)
        {
            var updatedAndNewOrders = newList.Select
                        (order => order with { OrderId = existingList.FirstOrDefault(g => g.clientEmail == order.clientEmail)?.OrderId ?? 0, IsUpdated = true });
            var oldOrders = existingList.Where(order => !newList.Any(g => g.clientEmail == order.clientEmail));
            var allOrders = updatedAndNewOrders.Union(oldOrders)
                                               .ToList()
                                               .AsReadOnly();
            return new PlacedOrderProducts(allOrders, newList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(), DateTime.Now);
        }

        public static IOrderProducts PlaceOrder(IOrderProducts products, IEnumerable<CalculatedOrderTotalPayment> newList, IEnumerable<CalculatedOrderTotalPayment> existingLis) => products.Match(
         whenUnvalidatedOrderProducts: unvalidatedClientOrder => unvalidatedClientOrder,
         whenInvalidOrderProducts: invalidatedClientOrder => invalidatedClientOrder,
         whenPlacedOrderProducts: placedOrder => placedOrder,
         whenValidatedOrderProducts: validatedOrder => validatedOrder,
         whenCalculatedOrderProducts: orders => MergeOrder(newList, existingLis)
         );

        */

        
        public static IOrderProducts PlaceOrder(ClientEmail client, IOrderProducts products) => products.Match(
            whenUnvalidatedOrderProducts: unvalidatedClientOrder => unvalidatedClientOrder,
            whenInvalidOrderProducts: invalidatedClientOrder => invalidatedClientOrder,
            whenPlacedOrderProducts: placedOrder => placedOrder,
            whenValidatedOrderProducts: validatedOrder => validatedOrder,
            whenCalculatedOrderProducts:calculated =>  GenerateExport(client, calculated)
            );


        private static IOrderProducts GenerateExport(ClientEmail client, CalculatedOrderProducts calculatedOrder)
        {
            decimal totalPrice = 0;
            foreach(CalculatedProductPrice product in calculatedOrder.ProductList)
            {
                totalPrice = totalPrice + product.totalPrice.Price;
            }
           return new PlacedOrderProducts(client, calculatedOrder.ProductList, new ProductPrice(totalPrice),
                calculatedOrder.ProductList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(),
                                    DateTime.Now);
        }

        private static StringBuilder CreateCsvLine(StringBuilder export, CalculatedProductPrice product) =>
           export.AppendLine($"{product.code.Value}, {product.quantity.Value}, {product.ProductId}, {product.totalPrice.ToString}");
    }
}
