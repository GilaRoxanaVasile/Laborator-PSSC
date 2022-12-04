using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static LanguageExt.Prelude;

namespace ProiectPSSC.Domain.Models
{
    public record ProductCode
    {
        public const string Pattern = "^XYZ[0-9]{4}$";
        private static readonly Regex ValidPattern = new(Pattern);

        public string Value { get; }

        public ProductCode(string value)
        {

            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidProductCodeException($"{value} is invalid");
            }
        }

        public static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public override string ToString()
        {
            return Value;
        }

        public static Option<ProductCode> TryParseProductCode(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<ProductCode>(new(stringValue));
            }
            else
            {
                return None;
            }
        }
        /*
        public decimal Code { get; }

        public ProductCode(string code)
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
        */
    }
}
