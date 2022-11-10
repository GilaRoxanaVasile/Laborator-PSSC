using Domain.Models;
using L02_PSSC.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static L02_PSSC.Domain.Cart;
using static L02_PSSC.Domain.Quantity;
using LanguageExt;
using static LanguageExt.Prelude;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using DataModels;
using Domain.Repositories;
using DataModels.Repositories;

namespace L02_PSSC
{
    class Program
    {

        private static readonly Random random = new Random();
        private static string ConnectionString= "Server=DESKTOP-MCMV9M2;Database=studenti;Trusted_Connection=True;MultipleActiveResultSets=true";
        static async Task Main(string[] args)
        {
            using ILoggerFactory loggerFactory = ConfigureLoggerFactory();
            ILogger<PlaceOrderWorkflow> logger=loggerFactory.CreateLogger<PlaceOrderWorkflow>();

            var clientMail = new ClientMail(ReadValue("Client Mail: "));
            Address clientAddress = new Address(ReadValue("Address: "));
            var client= new Client(clientMail, clientAddress);
            Guid cartID = Guid.NewGuid();
            var listOfProducts = ReadListOfProducts(cartID, client).ToArray();

            PlaceOrderCommand command = new(listOfProducts);
            var dbContextBuilder = new DbContextOptionsBuilder<ProductsContext>()
                                        .UseSqlServer(ConnectionString)
                                        .UseLoggerFactory(loggerFactory);
            ProductsContext productsContext = new ProductsContext(dbContextBuilder.Options);
            OrderrLineRepository orderLineRepository = new(productsContext);
            OrderHeadRepository orderHeadRepository = new(productsContext);
            ProductRepository productRepository = new(productsContext);
            //PlaceOrderCommand command = new(listOfProducts);
            PlaceOrderWorkflow workflow = new PlaceOrderWorkflow(logger,orderLineRepository,orderHeadRepository,productRepository);
            //var res = await workflow.ExecuteAsync(command);
            var result = await workflow.ExecuteAsync(client, cartID, command, CheckProductExists);

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
        private static ILoggerFactory ConfigureLoggerFactory()
        {
            return LoggerFactory.Create(builder => 
                                builder.AddSimpleConsole(options =>
                                {
                                    options.IncludeScopes = true;
                                    options.SingleLine = true;
                                    options.TimestampFormat = "hh:mm:ss ";
                                })
                                .AddProvider(new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()));
        }

        private static string? ReadValue(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }

        private static TryAsync<bool> CheckProductExists(ProductCode code)
        {
            Func<Task<bool>> func = async () =>
            {
                return true;
            };
            return TryAsync(func);
        }

    }
}