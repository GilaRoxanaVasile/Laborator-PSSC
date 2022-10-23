using Domain.Models;
using L02_PSSC.Domain;
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
        public static ICart ValidateCartProducts(Client client, Func<ProductCode, bool> checkProductExists, UnvalidatedCart cart)
        {
            List<ValidatedClientCart> validatedClientCart = new();
            bool isValidList = true;
            string invalidReason = string.Empty;
            foreach (var unvalidatedCartItem in cart.ProductList)
            {
                if (!ProductCode.TryParseProductCode(unvalidatedCartItem.productCode, unvalidatedCartItem.productPrice, out ProductCode code)
                    && checkProductExists(code))
                {
                    invalidReason = $"Invalid product code ({unvalidatedCartItem.productCode}, {unvalidatedCartItem})";
                    isValidList = false;
                    break;
                }

                if (!Quantity.TryParseQuantityUnit(unvalidatedCartItem.quantity, out QUnit quantity))
                {
                    invalidReason = $"Invalid quantity ({unvalidatedCartItem.quantity})";
                    isValidList = false;
                    break;
                }

                ValidatedClientCart validCart = new(client, unvalidatedCartItem.cartId, code, quantity);
                validatedClientCart.Add(validCart);
            }

            if (isValidList)
            {
                return new ValidatedCart(validatedClientCart);
            }
            else
            {
                return new InvalidatedCart(cart.ProductList, invalidReason);
            }
        }

        public static ICart CalculateTotalPrice(ICart cart) => cart.Match(
                whenEmptyCart: emptyCart => emptyCart,
                whenInvalidatedCart: invalidCart => invalidCart,
                whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
                whenCalculatedCartPrice: calculatedCart => calculatedCart,
                whenPayedCart: payedCart => payedCart,
                whenValidatedCart: validatedCart => 
                {
                    var calculatedPrice = validatedCart.ProductList.Select(
                        validatedCart =>
                        new CalculatedTotalPrice(
                                    validatedCart.client,
                                    validatedCart.idCart,
                                    validatedCart.productCode,
                                    validatedCart.quantity,
                                    validatedCart.productCode.Price * validatedCart.quantity.GetQ()));
                    return new CalculatedCartPrice(calculatedPrice.ToList().AsReadOnly());
                }
                );

        public static ICart SendOrder(ICart cart) => cart.Match(
              whenEmptyCart: emptyCart => emptyCart,
                whenInvalidatedCart: invalidCart => invalidCart,
                whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
                whenPayedCart: payedCart => payedCart,
                whenValidatedCart: validatedCart => validatedCart,
                whenCalculatedCartPrice: calculatedCart => 
                {
                    StringBuilder csv = new();
                    calculatedCart.productList.Aggregate(csv, (export, product) => export.AppendLine($"{product.client.clientMail.ToString()}, {product.client.address.ToString()}, {product.quantity.GetQ().ToString()}, {product.productCode.ToString()}, {product.totalPrice.ToString()}"));

                    PayedCart payedCart = new(calculatedCart.productList, csv.ToString(), DateTime.Now);
                    return payedCart;

                }
            );
            
    }
}
