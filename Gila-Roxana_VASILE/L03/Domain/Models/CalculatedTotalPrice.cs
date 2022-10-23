using L02_PSSC.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static L02_PSSC.Domain.Quantity;

namespace Domain.Models
{
    public record CalculatedTotalPrice(Client client, Guid idCart, ProductCode productCode, IQuantity quantity, decimal totalPrice);
}
