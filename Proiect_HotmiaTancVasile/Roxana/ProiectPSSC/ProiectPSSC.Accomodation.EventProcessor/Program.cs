using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using ProiectPSSC.Events.ServiceBus;
using ProiectPSSC.Accomodation.EventProcessor;
using ProiectPSSC.Events;

namespace ProiectPSSC.Accomodation.EventProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddAzureClients(builder =>
                {
                    builder.AddServiceBusClient(hostContext.Configuration.GetConnectionString("ServiceBus"));
                });

                services.AddSingleton<IEventListener, ServiceBusTopicEventListener>();
                services.AddSingleton<IEventHandler, OrderPlacedEventHandler>();

                services.AddHostedService<Worker>();
            });
    }
}