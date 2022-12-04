using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Models
{
    public record UnvalidatedClientOrder(string ClientEmail, string ProductCode, int Quantity);
    // public record UnvalidatedClientOrder(string ClientEmail, string ProductCode, int Quantity, decimal productPrice);

}
