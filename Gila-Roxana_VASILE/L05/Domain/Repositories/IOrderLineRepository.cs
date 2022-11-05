using Domain.Models;
using L02_PSSC.Domain;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static L02_PSSC.Domain.Cart;

namespace Domain.Repositories
{
    public interface IOrderLineRepository
    {
        TryAsync<List<CalculatedOrder>> TryGetExistingOders();
        TryAsync<Unit> TrySaveOrder(PayedCart carts);
    }
}
