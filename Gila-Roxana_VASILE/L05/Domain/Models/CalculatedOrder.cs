using L02_PSSC.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static L02_PSSC.Domain.Quantity;

namespace Domain.Models
{
    public record CalculatedOrder(ProductCode ProductCode, QUnit Quantity, ProductPrice Price)
    {
        public int OrderID { get; set; }
        public bool isUpdated { get; set; } 
    }
}
