using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace L02_PSSC.Domain
{
    public record ProductCode
    {
        //private static readonly Regex ValidPattrern = new("^x[0-9]{3}x$");

        public decimal Code { get; }

        public decimal Price { get; }

        public ProductCode(decimal code, decimal price)
        {
            if (IsValid(code) && IsValid(price))
            {
                Code=code;
                Price = price;
            }
            else
            {
                throw new Exception($"{code} or {price} invalid");
            }
        }


        public static bool TryParseProductCode(string priceString, string codeString, out ProductCode Code)
        {
            bool isValid = false;
            Code = null;
            if(decimal.TryParse(codeString, out decimal numericCode)&& decimal.TryParse(codeString, out decimal numericPrice))
            {
                if(IsValid(numericCode)&&IsValid(numericPrice))
                {
                    isValid = true;
                    Code = new(numericCode,numericPrice);
                }
            }
            return isValid;
        }

        public static bool IsValid(decimal numericCode) => numericCode > 0;

        public override string ToString()
        {
            return $"{Code:0.##}"+ $"{Price:0.##}";
        }
    }
}
