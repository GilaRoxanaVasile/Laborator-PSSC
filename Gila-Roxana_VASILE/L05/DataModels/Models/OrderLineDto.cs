using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class OrderLineDto
    {
        public int OrderLineId { get; set; }
        public int ProductId { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}
