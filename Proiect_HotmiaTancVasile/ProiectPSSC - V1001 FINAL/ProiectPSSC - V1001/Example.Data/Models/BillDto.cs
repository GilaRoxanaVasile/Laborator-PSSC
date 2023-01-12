using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data.Models
{
    public class BillDto
    {
        public int BillId { get; set; }
        public int ClientId { get; set; }
        public int OrderId { get; set; }
        public string BillAddress { get; set; }

        public string BillNumber { get; set; }
    }
}
