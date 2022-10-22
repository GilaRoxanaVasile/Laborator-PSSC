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

        public decimal Value { get; }

        public ProductCode(decimal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new Exception($"{value} is invalid");
            }
        }


        public static bool TryParseProductCode(string codeString, out ProductCode Code)
        {
            bool isValid = false;
            Code = null;
            if(decimal.TryParse(codeString, out decimal numericCode))
            {
                if(IsValid(numericCode))
                {
                    isValid = true;
                    Code = new(numericCode);
                }
            }
            return isValid;
        }

        public static bool IsValid(decimal numericCode) => numericCode > 0;

        public override string ToString()
        {
            return $"{Value:0.##}";
        }
    }
}
