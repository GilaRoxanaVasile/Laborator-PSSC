using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Models
{
    public record CalculatedOrderTotalPayment(ClientEmail clientEmail, CalculatedProductPrice product, ProductPrice totalPrice)
    {
        public int OrderId { get; set; } //ma gandesc
        public bool IsUpdated { get; set; }

    }
    /*
    public record CalculatedOrderTotalPayment(ClientEmail clientEmail, Quantity quantity, ProductPrice price,  ProductPrice totalPrice)
    {
        public int ProductId { get; set; } //ma gandesc
        public bool IsUpdated { get; set; }

    }
    */
}
