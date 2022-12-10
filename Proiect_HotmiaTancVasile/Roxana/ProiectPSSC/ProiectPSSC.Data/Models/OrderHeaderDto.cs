using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Data.Models
{
    public class OrderHeaderDto
    {
        public int OrderId { get; set; }
        public int ClientId { get; set; }
        public string ClientEmail { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentOption { get; set; } //ramburs sau card
        
        //public string CardDetails { get; set; }

    }
}
