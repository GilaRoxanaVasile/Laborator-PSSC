using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    public record PublishBillsCommand
    {
        public PublishBillsCommand(IReadOnlyCollection<UnvalidatedBill> inputBill)
        {
            InputBills = inputBill;
        }

        public IReadOnlyCollection<UnvalidatedBill> InputBills { get; }
    }
}
