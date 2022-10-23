using Domain.Models;
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
            PlaceOrderCommand command = new(listOfProducts);
            PlaceOrderWorkflow workflow = new PlaceOrderWorkflow();
            var result = workflow.Execute(client, cartID, command, (productcode) => true);

            result.Match(
                whenOrderPlacingFailedEvent: @event =>
                {
                    Console.WriteLine($"Placing the order failed: {@event.Reason}");
                    return @event;
                },
                whenOrderPlacingSucceded: @event =>
                {
                    Console.WriteLine($"Order placed.");
                    Console.WriteLine(@event.CSV);
                    return @event;
                }
             );

        }
 
        private static List<UnvalidatedClientCart> ReadListOfProducts(Guid cartID, Client client)
        {
            List<UnvalidatedClientCart> listOfProduct = new();
            
            do
            {
                var productCode = ReadValue("product code: ");
                if (string.IsNullOrEmpty(productCode))
                {
                    break;
                }

                var productPrice = ReadValue("product price: ");
                if (string.IsNullOrEmpty(productPrice))
                {
                    break;
                }

                var quantity = ReadValue("Cantitate: ");
                if (string.IsNullOrEmpty(quantity))
                {
                    break;
                }

               listOfProduct.Add(new (cartID, productCode, productPrice, quantity));
            } while (true);
            return listOfProduct;
        }
        
        private static string? ReadValue(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }
    }
}