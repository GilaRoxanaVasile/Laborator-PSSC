using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

namespace Domain.Models
{
    public record OrderID
    {
            //private static readonly Regex ValidPattrern = new("^x[0-9]{3}x$");

            public decimal Order { get; }

            public OrderID(decimal order)
            {
                if (IsValid(order))
                {
                    Order = order;
                }
                else
                {
                    throw new Exception($"{order} invalid");
                }
            }

            public static Option<OrderID> TryParseCode(string orderString)
            {
                if (decimal.TryParse(orderString, out decimal numericOrder) && IsValid(numericOrder))
                {
                    return Some<OrderID>(new(numericOrder));
                }
                else
                {
                    return None;
                }
            }

            public static bool TryParseProductCode(string orderString, out OrderID Order)
            {
                bool isValid = false;
                Order = null;
                if (decimal.TryParse(orderString, out decimal orderCode))
                {
                    if (IsValid(orderCode))
                    {
                        isValid = true;
                        Order = new(orderCode);
                    }
                }
                return isValid;
            }

            public static bool IsValid(decimal numericCode) => numericCode > 0;

            public override string ToString()
            {
                return $"{Order:0.##}";
            }
        }
    
}
