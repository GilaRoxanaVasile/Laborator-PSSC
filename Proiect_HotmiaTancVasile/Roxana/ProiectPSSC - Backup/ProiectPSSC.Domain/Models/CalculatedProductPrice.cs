﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Models
{
    public record CalculatedProductPrice(ProductCode code, Quantity quantity, ProductPrice price, ProductPrice totalPrice)
    {
        public int ProductId { get; set; } //ma gandesc
        public bool IsUpdated { get; set; }

    }

}