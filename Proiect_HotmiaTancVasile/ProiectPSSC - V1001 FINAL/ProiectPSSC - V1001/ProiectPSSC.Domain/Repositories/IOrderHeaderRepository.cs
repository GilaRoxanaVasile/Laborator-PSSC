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
    public interface IOrderHeaderRepository
    {
        TryAsync<List<CalculatedProductPrice>> TryGetExistingClientOrders();
        TryAsync<Unit> TrySaveOrders(PlacedOrderProducts order);
    }
}
