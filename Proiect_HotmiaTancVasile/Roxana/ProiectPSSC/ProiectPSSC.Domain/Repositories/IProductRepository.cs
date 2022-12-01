using LanguageExt;
using ProiectPSSC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Repositories
{
    public interface IProductRepository
    {
        TryAsync<List<ProductCode>> TryGetExistingProducts(IEnumerable<string> productsToCheck);
        TryAsync<List<Products>> TryGetProductCatalog(IEnumerable<string> productCode);
    }
}
