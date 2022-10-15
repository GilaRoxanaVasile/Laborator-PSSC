using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01_PSSC.Domain
{
    public record ListOfProducs(string ProductCode, Quantity.IQuantity ProdQunatity);
    /*
    {
        private string ProductCode { get; }
        private Quantity ProductQuantity { get; }
    }
    */
}
