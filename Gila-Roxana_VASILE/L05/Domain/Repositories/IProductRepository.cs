using L02_PSSC.Domain;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        TryAsync<List<ProductCode>> TryGetExistingProducts(IEnumerable<decimal> productsToCheck);
    }
}
