using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Models
{
    public record SendOrderCommand
    {
        public SendOrderCommand(IReadOnlyCollection<UnvalidatedClientOrder> inputClientProducts)
        {
            InputClientProducts = inputClientProducts;
        }
        public IReadOnlyCollection<UnvalidatedClientOrder> InputClientProducts { get; }
    }
}
