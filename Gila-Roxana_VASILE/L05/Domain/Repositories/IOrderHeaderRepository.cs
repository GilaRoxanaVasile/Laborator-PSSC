using Domain.Models;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static L02_PSSC.Domain.Cart;

namespace Domain.Repositories
{
    public interface IOrderHeaderRepository
    {
        TryAsync<List<OrderID>> TryGetExistingOrderss(IEnumerable<decimal> ordersToCheck);
    }
}
