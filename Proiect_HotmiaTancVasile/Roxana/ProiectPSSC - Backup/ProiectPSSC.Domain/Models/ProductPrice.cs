using LanguageExt;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

namespace ProiectPSSC.Domain.Models
{
    public record ProductPrice
    {
        public decimal Price { get; }
        public ProductPrice(decimal price)
        {
            if (IsValid(price))
            {

                Price = price;
            }
            else
            {
                throw new Exception($"{price} invalid");
            }
        }
        public static bool IsValid(decimal numericPrice) => numericPrice > 0;

        public static Option<ProductPrice> TryParsePrice(decimal numericPrice)
        {
            if (IsValid(numericPrice))
            {
                return Some<ProductPrice>(new(numericPrice));
            }
            else
            {
                return None;
            }
        }
        public static Option<ProductPrice> TryParsePrice(string priceString)
        {
            if (decimal.TryParse(priceString, out decimal numericPrice) && IsValid(numericPrice))
            {
                return Some<ProductPrice>(new(numericPrice));
            }
            else
            {
                return None;
            }
        }

    }
}
