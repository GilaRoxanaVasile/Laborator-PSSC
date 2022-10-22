using L02_PSSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static L02_PSSC.Domain.Cart;
using static L02_PSSC.Domain.Quantity;

namespace Domain
{
    public static class CartOperation
    {
        public static ICart ValidateCartProducts(Client client, Func<ProductCode,bool> checkProductExists, UnvalidatedCart cart)
        {
            List<ValidatedClientCart> validatedClientCart = new ();
            bool isValidList = true;
            string invalidReason = string.Empty;
            foreach(var unvalidatedCartItem in cart.ProductList)
            {
                if (!ProductCode.TryParseProductCode(unvalidatedCartItem.productCode, out ProductCode code)
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
           
            if(isValidList)
            {
                return new ValidatedCart(validatedClientCart);
            }
            else
            {
                return new InvalidatedCart(cart.ProductList, invalidReason);
            }
        }
    }
}
