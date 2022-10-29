using L02_PSSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public record PlaceOrderCommand
    {
        public PlaceOrderCommand(IReadOnlyCollection<UnvalidatedClientCart> inputProducts)
        {
            InputProducts = inputProducts;
        }
        public IReadOnlyCollection<UnvalidatedClientCart> InputProducts { get; }
    }
}
