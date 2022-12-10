using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Models
{
    public record CalculatedProductPrice(ClientEmail clientEmail, ProductCode code, Quantity quantity, ProductPrice price, ProductPrice totalPrice)
    {
        public int OrderLineId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; } //ma gandesc
        public int ClientId { get; set; }
        public bool IsUpdated { get; set; }

    }

}
