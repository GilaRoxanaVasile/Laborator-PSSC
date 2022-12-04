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

namespace ProiectPSSC.Data.Repositories
{
    public class OrderLineRepository : IOrderLineRepository
    {
        private readonly OrderContext _orderContext;
        public OrderLineRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public TryAsync<List<ClientProduct>> TryGetExistingClientProducts(IEnumerable<string> productCode, IEnumerable<int> quantity) => async () =>
        {
            var clientProducts = await _orderContext.OrderLines
                                .Where(clientProd => productCode.Contains(clientProd.ProductCode))
                                .AsNoTracking()
                                .ToListAsync();
            return clientProducts.Select(clientProd =>
                        new ClientProduct
                        (new ProductCode(clientProd.ProductCode), 
                        new Quantity(clientProd.Quantity)))
                                    .ToList();
        };

        public TryAsync<List<CalculatedProductPrice>> TryGetExistingOrderProducts()
        {
            throw new NotImplementedException();
        }

        public TryAsync<Unit> TrySaveProducts(OrderProducts.PlacedOrderProducts order)
        {
            throw new NotImplementedException();
        }
    }
}
