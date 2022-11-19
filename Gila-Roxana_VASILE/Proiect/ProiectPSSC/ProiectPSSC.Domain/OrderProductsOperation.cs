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
        public static Task<IOrderProducts> ValidateOrder(Func<ClientEmail, Option<ClientEmail>> checkClientExists, UnvalidatedOrderProducts orderProducts) =>
            orderProducts.ProductList
                        .Select(ValidateOrderProducts(checkClientExists))
                        .Aggregate(CrateEmptyValidatedOrderProductsList().ToAsync(), ReduceValidProducts)
                        .MatchAsync(
                            Right: validatedOrderProducts => new ValidatedOrderProducts(validatedOrderProducts),
                            LeftAsync: errorMessage => Task.FromResult((IOrderProducts) new InvalidOrderProducts(orderProducts.ProductList, errorMessage))
                        );
        private static Func<UnvalidatedClientOrder, EitherAsync<string, ValidatedClientOrder>> ValidateOrderProducts(Func<ClientEmail, Option<ClientEmail>> checkClientExists) =>
            unvalidatedClientProducts => ValidateOrderProducts(checkClientExists, unvalidatedClientProducts);

        private static EitherAsync<string, ValidatedClientOrder> ValidateOrderProducts(Func<ClientEmail, Option<ClientEmail>> checkClientExists, UnvalidatedClientOrder unvalidatedClientOrder) =>
            from orderProductCode in ProductCode.TryParseProductCode(unvalidatedClientOrder.ProductCode)
                                            .ToEitherAsync($"Invalid product code ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.ProductCode})")
            from quantity in Quantity.TryParseQuantity(unvalidatedClientOrder.Quantity)
                                      .ToEitherAsync($"Invalid quantity ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.Quantity})")
            from clientEmail in ClientEmail.TryParseClientEmail(unvalidatedClientOrder.ClientEmail)
                                .ToEitherAsync($"Invalid client email ({unvalidatedClientOrder.ClientEmail})")
            from price in ProductPrice.TryParsePrice(unvalidatedClientOrder.productPrice)
                            .ToEitherAsync($"Invalid product code ({unvalidatedClientOrder.ClientEmail}, {unvalidatedClientOrder.ProductCode})")
            from clientExists in checkClientExists(clientEmail)
                                 .ToEitherAsync($"Client {clientEmail.Value} does not exist.")
            select new ValidatedClientOrder(clientEmail, orderProductCode, quantity, price);


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

        public static IOrderProducts CalculateFinalPrices(IOrderProducts orderProducts) => orderProducts.Match(
            whenUnvalidatedOrderProducts: unvalidatedClientOrder => unvalidatedClientOrder,
            whenInvalidOrderProducts: invalidatedClientOrder => invalidatedClientOrder,
            whenPlacedOrderProducts: placedOrder => placedOrder,
            whenCalculatedOrderProducts: calculatedOrderProducts => calculatedOrderProducts,
            whenValidatedOrderProducts: CalculateFinalPrice
            );

        private static IOrderProducts CalculateFinalPrice(ValidatedOrderProducts validOrder) =>
            new CalculatedOrderProducts(validOrder.ProductList
                                                   .Select(CalculateFinalProductPrice)
                                                   .ToList()
                                                   .AsReadOnly());

        private static CalculatedOrderTotalPayment CalculateFinalProductPrice(ValidatedClientOrder validatedClientOrder)
            => new CalculatedOrderTotalPayment(validatedClientOrder.clientEmail,
                validatedClientOrder.quantity, validatedClientOrder.price, new ProductPrice(validatedClientOrder.price.Price * validatedClientOrder.quantity.Value));


        private static IOrderProducts MergeProducts(IOrderProducts products, IEnumerable<CalculatedOrderTotalPayment> existingProducts) =>
            products.Match(
            whenUnvalidatedOrderProducts: unvalidatedClientOrder => unvalidatedClientOrder,
            whenInvalidOrderProducts: invalidatedClientOrder => invalidatedClientOrder,
            whenPlacedOrderProducts: placedOrder => placedOrder,
            whenValidatedOrderProducts: validatedOrder => validatedOrder,
            whenCalculatedOrderProducts: calculatedOrderProducts => MergeProducts(calculatedOrderProducts.ProductList, existingProducts)
                );

        private static CalculatedOrderProducts MergeProducts(IEnumerable<CalculatedOrderTotalPayment> newList, IEnumerable<CalculatedOrderTotalPayment> existingList)
        {
            var updatedAndNewProducts = newList.Select(product => product with { ProductId = existingList.FirstOrDefault(g => g.clientEmail == product.clientEmail)?.ProductId ?? 0, IsUpdated = true });
            var oldProducts = existingList.Where(product => !newList.Any(g => g.clientEmail == product.clientEmail));
            var allProducts = updatedAndNewProducts.Union(oldProducts)
                                               .ToList()
                                               .AsReadOnly();
            return new CalculatedOrderProducts(allProducts);
        }

        public static IOrderProducts PlaceORder(IOrderProducts products) => products.Match(
            whenUnvalidatedOrderProducts: unvalidatedClientOrder => unvalidatedClientOrder,
            whenInvalidOrderProducts: invalidatedClientOrder => invalidatedClientOrder,
            whenPlacedOrderProducts: placedOrder => placedOrder,
            whenValidatedOrderProducts: validatedOrder => validatedOrder,
            whenCalculatedOrderProducts: GenerateExport
            );

        private static IOrderProducts GenerateExport(CalculatedOrderProducts calculatedOrder)=>
            new PlacedOrderProducts(calculatedOrder.ProductList,
                calculatedOrder.ProductList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(),
                                    DateTime.Now);

        private static StringBuilder CreateCsvLine(StringBuilder export, CalculatedOrderTotalPayment order) =>
           export.AppendLine($"{order.clientEmail.Value}, {order.ProductId}, {order.quantity.Value}, {order.totalPrice.ToString}");
    }
}
