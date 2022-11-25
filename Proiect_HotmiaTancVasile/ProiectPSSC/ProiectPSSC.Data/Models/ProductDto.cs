using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Data.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Code { get; set; }
        public int Stoc { get; set; }
        public decimal Pret { get; set; }
    }
}
