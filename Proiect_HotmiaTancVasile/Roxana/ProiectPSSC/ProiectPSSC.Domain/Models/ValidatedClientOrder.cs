using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Models
{
    public record ValidatedClientOrder(ClientEmail clientEmail, ProductCode productCode, Quantity quantity, ProductPrice price);

}
