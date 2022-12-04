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
    public class ProductRepository : IProductRepository
    {
        private readonly OrderContext orderContext;

        public ProductRepository(OrderContext orderContext)
        {
            this.orderContext = orderContext;
        }

        public TryAsync<List<ProductCode>> TryGetExistingProducts(IEnumerable<string> productsToCheck) => async () =>
        {
            var products = await orderContext.Products
                            .Where(product => productsToCheck.Contains(product.Code))
                            .AsNoTracking()
                            .ToListAsync();
            return products.Select(product => new ProductCode(product.Code))
                        .ToList();
        };

        public TryAsync<List<Products>> TryGetProductCatalog(IEnumerable<string> productCode) => async () =>
        {
            var catalog = await orderContext.Products
                            .Where(product => productCode.Contains(product.Code))
                            .AsNoTracking()
                            .ToListAsync();
            return catalog.Select(
                product =>  new Products(new ProductCode(product.Code),
                new Quantity(product.Stoc), new ProductPrice(product.Price)))
                    .ToList();
        };

        public TryAsync<List<Quantity>> TryGetProductStoc(IEnumerable<string> productCode) => async () =>
        {
            var catalog = await orderContext.Products
                            .Where(product => productCode.Contains(product.Code))
                            .AsNoTracking()
                            .ToListAsync();
            return catalog.Select(
                product => new Quantity(product.Stoc))
                            .ToList();
        };

        public TryAsync<List<ProductPrice>> TryGetProductPrices(IEnumerable<string> productCode) => async () =>
        {
            var catalog = await orderContext.Products
                            .Where(product => productCode.Contains(product.Code))
                            .AsNoTracking()
                            .ToListAsync();
            return catalog.Select(
                product => new ProductPrice(product.Price))
                            .ToList();
        };
    }
}
