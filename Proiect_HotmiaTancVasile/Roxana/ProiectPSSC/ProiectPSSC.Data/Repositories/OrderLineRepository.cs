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
     CREATE TABLE [dbo].[OrderLine](
	[OrderLineId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Code] [varchar](7) NOT NULL,
	[Quantity] [int] NULL,
     */

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

        public TryAsync<List<CalculatedProductPrice>> TryGetExistingOrderProducts() => async () => (await (
            from oh in _orderContext.OrderHeaders
            from p in _orderContext.Products
            join oLine2 in _orderContext.OrderLines on p.ProductId equals oLine2.ProductId
            join oLine in _orderContext.OrderLines on oh.OrderId equals oLine.OrderId
            select new { oh.ClientEmail, oLine2.ProductCode, oLine2.Quantity, p.Price, p.ProductId, oh.ClientId })
            .AsNoTracking()
            .ToListAsync())
            .Select(result => new CalculatedProductPrice(
                clientEmail: new(result.ClientEmail),
                code: new(result.ProductCode),
                quantity: new(result.Quantity),
                price: new(result.Price),
                totalPrice: new(result.Price * result.Quantity))
                {
                    ClientId = result.ClientId
                    //ProductId = result.ProductId
                })
                .ToList();

        public TryAsync<Unit> TrySaveProducts(OrderProducts.PlacedOrderProducts order) => async () =>
        {
            var products = (await _orderContext.Products.ToListAsync()).ToLookup(product => product.ProductCode);
            var orderHeader = (await _orderContext.OrderHeaders.ToListAsync()).ToLookup(clientOrder => clientOrder.ClientEmail);
            var newOrderProducts = order.ProductList
                                    .Where(p => p.IsUpdated && p.OrderLineId == 0)
                                    .Select(p => new OrderLineDto()
                                    {
                                         OrderId = orderHeader[p.code.Value].Single().OrderId,
                                         ProductId = p.ProductId,
                                         ProductCode = p.code.Value,
                                         Quantity = p.quantity.Value,
                                    });
           var updatedOrderProducts = order.ProductList.Where(p => p.IsUpdated && p.ProductId > 0)
                                        .Select(p => new OrderLineDto()
                                        {
                                          OrderLineId = p.OrderLineId,
                                          OrderId = orderHeader[p.code.Value].Single().OrderId,
                                          ProductId = p.ProductId,
                                          ProductCode = p.code.Value,
                                          Quantity = p.quantity.Value,
                                        });

            _orderContext.AddRange(newOrderProducts);
            foreach ( var entity in updatedOrderProducts)
            {
                _orderContext.Entry(entity).State=EntityState.Modified;
            }

            await _orderContext.SaveChangesAsync();

            return unit;
                                    
        };
    }
}
