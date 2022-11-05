using Domain.Models;
using L02_PSSC.Domain;
using LanguageExt;
using static LanguageExt.Prelude;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Models.OrderPlacedEvent;
using static L02_PSSC.Domain.Cart;
using static L02_PSSC.Domain.Quantity;

namespace Domain
{
    public static class CartOperation
    {
        public static Task<ICart> ValidateCartProducts(Client client, Guid id, Func<ProductCode, TryAsync<bool>> checkProductExists, UnvalidatedCart cart) =>
                cart.ProductList
                    .Select(ValidateClientCart(client, id, checkProductExists))
                    .Aggregate(CreateEmptyValidatedProductList().ToAsync(), ReduceValidProducts)
                    .MatchAsync(
                        Right: validatedCart=> new ValidatedCart(validatedCart),
                        LeftAsync: errorMessage => Task.FromResult((ICart)new InvalidatedCart(cart.ProductList, errorMessage)) 
                    );
     


        private static Func<UnvalidatedClientCart, EitherAsync<string, ValidatedClientCart>> ValidateClientCart(Client client, Guid id, Func<ProductCode, TryAsync<bool>> checkProdExists) =>
                unvalidatedProductList => ValidateClientCart(client, id, checkProdExists, unvalidatedProductList);

        private static EitherAsync<string, ValidatedClientCart> ValidateClientCart(Client client, Guid idCart,Func<ProductCode, TryAsync<bool>> checkProdExists, UnvalidatedClientCart cart) =>
             from code in ProductCode.TryParseCode(cart.productCode)
                                      .ToEitherAsync(() => $"Invalid product code ({cart.productCode}")
             from price in ProductPrice.TryParsePrice(cart.productPrice)
                                        .ToEitherAsync(() => $"Invalid price ({cart.productPrice}")
             from quantity in QUnit.TryParseQuantity(cart.quantity)
                                        .ToEitherAsync(() => $"Invalud price ({cart.quantity})")
             from productExists in checkProdExists(code)
                                        .ToEither(error => error.ToString())
             select new ValidatedClientCart(client, idCart, new Product(code, quantity, price));


        private static Either<string, List<ValidatedClientCart>> CreateEmptyValidatedProductList() =>
            Right(new List<ValidatedClientCart>());

        private static EitherAsync<string, List<ValidatedClientCart>> ReduceValidProducts(EitherAsync<string, List<ValidatedClientCart>> acc, EitherAsync<string, ValidatedClientCart> next) =>
            from list in acc
            from nextProduct in next
            select list.AppendValidProduct(nextProduct);


        private static List<ValidatedClientCart> AppendValidProduct(this List<ValidatedClientCart> list, ValidatedClientCart valid)
        {
            list.Add(valid);
            return list;
        }

        public static ICart CalculateFinalPrices(ICart cart) =>
            cart.Match(
                whenEmptyCart: emptyCart => emptyCart,
                whenInvalidatedCart: invalidCart => invalidCart,
                whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
                whenCalculatedCartPrice: calculatedCart => calculatedCart,
                whenPayedCart: payedCart => payedCart,
                whenValidatedCart: CalculateFinalPrice
                );

        private static ICart CalculateFinalPrice(ValidatedCart cart) =>
            new CalculatedCartPrice(cart.ProductList
                                        .Select(CalculatedCartFinalPrice)
                                        .ToList()
                                        .AsReadOnly()
                );

        private static CalculatedTotalPrice CalculatedCartFinalPrice(ValidatedClientCart cart) =>
            new CalculatedTotalPrice(cart.client, cart.idCart, cart.product,new ProductPrice (cart.product.price.Price*cart.product.quantity.GetQ()));


        public static ICart SendOrder(ICart cart) =>
            cart.Match(
                whenEmptyCart: emptyCart => emptyCart,
                whenInvalidatedCart: invalidCart => invalidCart,
                whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
                whenPayedCart: payedCart => payedCart,
                whenValidatedCart: validatedCart => validatedCart,
                whenCalculatedCartPrice: calculatedCart => GenerateExport(calculatedCart)
            );

        private static ICart GenerateExport(CalculatedCartPrice calculatedCart) =>
            new PayedCart(calculatedCart.productList, calculatedCart.productList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(), DateTime.Now);

        private static StringBuilder CreateCsvLine(StringBuilder export, CalculatedTotalPrice total) =>
            export.AppendLine($"{total.idCart}, {total.productList})");


    }
}
