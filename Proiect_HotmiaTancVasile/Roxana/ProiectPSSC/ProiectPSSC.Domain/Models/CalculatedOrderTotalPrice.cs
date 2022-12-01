using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Models
{
    public record CalculatedOrderTotalPrice(ClientEmail clientEmail,  ProductPrice totalPrice)
    {
        public int ClientId { get; set; } //ma gandesc
        public bool IsUpdated { get; set; }

    }
}
