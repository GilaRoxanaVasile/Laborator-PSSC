using L02_PSSC.Domain;
using System;
using System.Collections.Generic;
using static L02_PSSC.Domain.Cart;

namespace L02_PSSC
{
    class Program
    {
        private static readonly Random random = new Random();
        static void Main(string[] args)
        {
            var clientID = ReadValue("Client ID: ");
            var client = new ClientID(clientID);
            var ClientList = new List<Client>();
            var listOfProducts = ReadListOfProducts().ToArray();
            Cart.UnvalidatedCart unvalidatedProduct = new(listOfProducts);
            ICart result = ValidateCart(unvalidatedProduct);
            // ClientID client = ClientID(Console.ReadLine());
            //var clientID = ReadValue("Client ID: ");
            result.Match(
                whenEmptyCart: emptyResult => emptyResult,
                whenUnvalidatedCart: unvalidatedCart => unvalidatedProduct,
                whenValidatedCart: validatedResult => PayCart(validatedResult),
                whenPayedCart: payedCart => payedCart //ClientList.Add(new(client, listOfProducts))//payedCart //?
                );
        }

        private static void ClientHistory(ClientID clientID,Product[] prod)
        {
            //adaauga in istoric comenzi clientul si cosul
        }

        private static List<Product> ReadListOfProducts()
        {
            List<Product> listOfProduct = new();
            do
            {
                var productCode = ReadValue("Product Code: ");
                if (string.IsNullOrEmpty(productCode))
                {
                    break;
                }

                var quantity = ReadValue("Quantity: ");
                if (string.IsNullOrEmpty(quantity))
                {
                    break;
                }

                var address = ReadValue("Address: ");
                if (string.IsNullOrEmpty(address))
                {
                    break;
                }

                listOfProduct.Add(new Product(quantity, productCode, address));
            } while (true);
            return listOfProduct;
        }

        private static ICart ValidateCart(Cart.UnvalidatedCart unvalidatedCart) =>
            random.Next(100) > 50 ? 
            throw new Exception("Random error") 
            : new Cart.ValidatedCart(new List<Product>());


        private static ICart PayCart(Cart.ValidatedCart validatedCart) =>
            new PayedCart(new List<Product>());

        private static string? ReadValue(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }
    }
}