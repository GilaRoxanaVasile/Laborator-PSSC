using LanguageExt;
using static LanguageExt.Prelude;
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

        public ProductCode(decimal code)
        {
            if (IsValid(code))
            {
                Code = code;
            }
            else
            {
                throw new Exception($"{code} invalid");
            }
        }

        public static Option<ProductCode> TryParseCode(string codeString)
        {
            if (decimal.TryParse(codeString, out decimal numericCode) && IsValid(numericCode))
            {
                return Some<ProductCode>(new(numericCode));
            }
            else
            { 
                return None; 
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
            return $"{Code:0.##}";
        }
    }
}
