using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ProiectPSSC.Domain.Models
{
    public record ProductCode
    {
        public const string Pattern = "^XYZ[0-9]{4}$";
        private static readonly Regex PatternRegex = new(Pattern);

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

        public static Option<ProductCode> TryParseProductCode(string codeString)
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
            if (decimal.TryParse(codeString, out decimal numericCode))
            {
                if (IsValid(numericCode))
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
