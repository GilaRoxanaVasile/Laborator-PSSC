using Domain.Repositories;
using L02_PSSC.Domain;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductsContext dbContext;
        public ProductRepository(ProductsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TryAsync<List<ProductCode>> TryGetExistingProducts(IEnumerable<decimal> productsToCheck) => async () =>
        {
            var products = await dbContext.Products
                                        .Where(product => productsToCheck.Contains(product.ProductId))
                                         .AsNoTracking()
                                         .ToListAsync();
            return products.Select(products => new ProductCode(products.ProductId)).ToList();
        };
    }
}
