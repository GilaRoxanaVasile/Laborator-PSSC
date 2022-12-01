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
    public record Quantity
    {
        public int Value { get; }
        public Quantity(int value)
        {
            if(IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidQuantityException($"{value:0.##} is an invalid quantity value.");
            }
        }

        public static Quantity operator +(Quantity a, Quantity b) => new Quantity(a.Value + b.Value);

        public override string ToString()
        {
            return $"{Value:0.##}";
        }

        public static Option<Quantity> TryParseQuantity(int numericQuantity)
        {
            if (IsValid(numericQuantity))
            {
                return Some<Quantity>(new(numericQuantity));
            }
            else
            {
                return None;
            }
        }
        public static Option<Quantity> TryParseQuantity(string quantityString)
        {
            if (int.TryParse(quantityString, out int numericQuantity) && IsValid(numericQuantity))
            {
                return Some<Quantity>(new(numericQuantity));
            }
            else
            {
                return None;
            }
        }

        private static bool IsValid(int numericQuantity) => numericQuantity > 0;

    }
}
