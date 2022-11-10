using Domain.Models;
using Domain.Repositories;
using L02_PSSC.Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static L02_PSSC.Domain.Quantity;

namespace DataModels.Repositories
{
    public class OrderrLineRepository : IOrderLineRepository
    {

        private readonly ProductsContext dbContext;
        public OrderrLineRepository(ProductsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<CalculatedOrder>> TryGetExistingOders()
        {
            throw new NotImplementedException();
        }

        /*
        public TryAsync<List<CalculatedOrder>> TryGetExistingOders() => async () => (await (
            from p in dbContext.Products
            from oHead in dbContext.OrderHeaders
            join oLine in dbContext.OrderLines on oHead.OrderId equals oLine.OrderLineId
           // join oLine2 in dbContext.OrderLines on p.ProductId equals oLine2.ProductId
            select new { p.ProductId, p.Stoc, oHead.Total })
            .AsNoTracking()
            .ToListAsync())
             .Select(result => new CalculatedOrder(
                 ProductCode: new(result.ProductId),
                 QUnit: new(result.Stoc ?? 0m),
                 ProductPrice: new(result.Total) ?? 0m)
               )
             .ToList();
        
    */
        public TryAsync<Unit> TrySaveOrder(Cart.PayedCart carts)
        {
            throw new NotImplementedException();
        }
    }
}
