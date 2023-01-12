using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Exemple.Domain.Models
{
    public record BillAddress
    {


        public string Value { get; }

        internal BillAddress(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new InvalidGradeException($"{value:0.##} is an invalid grade value.");
            }


        }
        public static Option<BillAddress> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<BillAddress>(new(stringValue));
            }
            else
            {
                return None;
            }
        }
        private static bool IsValid(string numericGrade) => numericGrade != "";
    }
}
