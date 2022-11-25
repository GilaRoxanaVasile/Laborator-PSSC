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
    public class OrderLineRepository : IOrderLineRepository
    {
        public TryAsync<List<int>> TryGetExistingOrders(IEnumerable<int> orders)
        {
            throw new NotImplementedException();
        }
    }
}
