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

namespace DData.Repositories
{
    public class OrderHeadRepository : IOrderHeaderRepository
    {

        private readonly ProductsContext _productsContext;
        public OrderHeadRepository(ProductsContext productsContext)
        {
            _productsContext = productsContext;
        }

        public TryAsync<List<OrderID>> TryGetExistingOrderss(IEnumerable<decimal> ordersToCheck) => async () =>
        {
            var orders = await _productsContext.OrderHeaders
                                       .Where(order => ordersToCheck.Contains(order.OrderId))
                                        .AsNoTracking()
                                        .ToListAsync();
            return orders.Select(order => new OrderID(order.OrderId))
                        .ToList();
        };
    }
}
