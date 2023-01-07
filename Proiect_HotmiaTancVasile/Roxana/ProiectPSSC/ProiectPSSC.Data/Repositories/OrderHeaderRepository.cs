using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;
using ProiectPSSC.Domain.Models;
using ProiectPSSC.Domain.Repositories;
using LanguageExt;
using ProiectPSSC.Data.Models;

namespace ProiectPSSC.Data.Repositories
{
    /*
     CREATE TABLE [dbo].[OrderHeader](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
    [ClientEmail] [varchar](20) NOT NULL,
	[TotalPrice] [decimal] NOT NULL,
	[PaymentOption] [varchar](20) NOT NULL,
     */

    public class OrderHeaderRepository : IOrderHeaderRepository
    {
        private readonly OrderContext dbContext;
	public OrderHeaderRepository(OrderContext ctx)
        {
            dbContext = ctx;
        }

        public TryAsync<List<CalculatedOrderTotalPrice>> TryGetExistingClientOrders() => async () => (await (
            from c in dbContext.Clients
            from ol in dbContext.OrderLines
            join oh in dbContext.OrderHeaders on c.ClientId equals oh.ClientId
            select new { oh.ClientEmail, oh.TotalPrice, oh.ClientId })
            .AsNoTracking()
            .ToListAsync())
            .Select(result => new CalculatedOrderTotalPrice(
                clientEmail: new(result.ClientEmail),
            totalPrice: new(result.TotalPrice))
            {
                ClientId = result.ClientId
            })
            .ToList();

        public TryAsync<Unit> TrySaveOrders(OrderProducts.PlacedOrderProducts order) => async () =>
        {
            var clients = (await dbContext.Clients.ToListAsync()).ToLookup(client => client.ClientEmail);
            var newOrderProducts = order.ProductList
            .Where(p => p.IsUpdated && p.OrderId == 0)
            .Select(p => new OrderHeaderDto()
            {
                  ClientId = clients[p.clientEmail.Value].Single().ClientId,
                  ClientEmail = p.clientEmail.Value,
                  TotalPrice = p.totalPrice.Price,
                  PaymentOption = "ramburs", //tiganie
            });

            var updatedOrderProducts = order.ProductList.Where(p => p.IsUpdated && p.OrderId > 0)
                .Select(p => new OrderHeaderDto()
                {
                    OrderId = p.OrderId,
                    ClientId = clients[p.clientEmail.Value].Single().ClientId,
                    ClientEmail = p.clientEmail.Value,
                    TotalPrice = p.totalPrice.Price,
                    PaymentOption = "ramburs", //tiganie
                });

            dbContext.AddRange(newOrderProducts);
            foreach (var entity in updatedOrderProducts)
            {
                dbContext.Entry(entity).State = EntityState.Modified;
            }

           //await dbContext.SaveChangesAsync();
            
            try
            {
                // Attempt to save changes to the database
                await dbContext.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is OrderHeaderDto)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];

                            // TODO: decide which value should be written to database
                            // proposedValues[property] = <value to be saved>;
                        }

                        // Refresh original values to bypass next concurrency check
                        entry.OriginalValues.SetValues(databaseValues);
                    }
                    else
                    {
                        throw new NotSupportedException(
                            "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name);
                    }
                }
            }
            
            return unit;
        };
    }
}
