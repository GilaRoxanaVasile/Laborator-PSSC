using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Repositories
{
    public interface IOrderLineRepository
    {
        TryAsync<List<int>> TryGetExistingOrders(IEnumerable<int> orders);
    }
}
