using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L02_PSSC.Domain
{
    public record Product
    {
        public string ProductQuantity { get; set; }
        public string ProductCode { get; set; }
        public string Address { get; set; }
        public Product(string quantity, string code, string addr)
        {
            ProductQuantity = quantity;
            ProductCode = code;
            Address = addr;
        }
        public override string ToString()
        {
            return ProductQuantity + " " + ProductCode + " " + Address;
        }
    }
}
