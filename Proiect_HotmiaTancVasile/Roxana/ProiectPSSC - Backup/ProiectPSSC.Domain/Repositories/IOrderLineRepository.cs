using LanguageExt;
using ProiectPSSC.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProiectPSSC.Domain.Models.OrderProducts;

namespace ProiectPSSC.Domain.Repositories
{
    public interface IOrderLineRepository
    {
        TryAsync<List<CalculatedProductPrice>> TryGetExistingOrderProducts();
        TryAsync<Unit> TrySaveProducts(PlacedOrderProducts order);
      //  TryAsync<List<ClientProduct>> TryGetExistingClientProducts(IEnumerable<string> productCode, IEnumerable<int> quantity);
    }
}
