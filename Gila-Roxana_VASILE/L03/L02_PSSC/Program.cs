using L02_PSSC.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static L02_PSSC.Domain.Cart;
using static L02_PSSC.Domain.Quantity;

namespace L02_PSSC
{
    class Program
    {

        private static readonly Random random = new Random();
        static void Main(string[] args)
        {
            var clientMail = new ClientMail(ReadValue("Client Mail: "));

            Address clientAddress = new Address(ReadValue("Address: "));

            var client= new Client(clientMail, clientAddress);

            Guid cartID = Guid.NewGuid();

            var listOfProducts = ReadListOfProducts(cartID, client).ToArray();

            EmptyCart emptyCart = new(cartID);
            ICart result = ValidateCart(emptyCart, client);
            result.Match(
                whenEmptyCart: emptyResult => emptyResult,
                whenUnvalidatedCart: unvalidatedCart => unvalidatedCart,
                whenInvalidatedCart: invalidatedCart => invalidatedCart,
                whenValidatedCart: validatedResult => PayCart(validatedResult),
                whenPayedCart: payedCart => payedCart 
                );
        }
 
        private static List<UnvalidatedClientCart> ReadListOfProducts(Guid cartID, Client client)
        {
            List<UnvalidatedClientCart> listOfProduct = new();
            
            do
            {
                ///decimal productCode;
                // bool ok = new ProductCode(ProductCode.TryParseProductCode(ReadValue("Product Code: "),out ProductCode productCode));
                var productCode = ReadValue("product code: ");
                if (string.IsNullOrEmpty(productCode))
                {
                    break;
                }

                var quantity = ReadValue("Cantitate: ");
                if (string.IsNullOrEmpty(quantity))
                {
                    break;
                }

               listOfProduct.Add(new (cartID, productCode, quantity));
            } while (true);
            return listOfProduct;
        }

        private static ICart ValidateCart(Cart.EmptyCart emptyCart, Client client) =>
            random.Next(100) > 50 ? 
            throw new Exception("Random error") 
            : new Cart.ValidatedCart(new List <ValidatedClientCart> ());


        private static ICart PayCart(Cart.ValidatedCart validatedCart) =>
            new PayedCart(new List<ValidatedClientCart>(), DateTime.Now);

        private static string? ReadValue(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }
    }
}