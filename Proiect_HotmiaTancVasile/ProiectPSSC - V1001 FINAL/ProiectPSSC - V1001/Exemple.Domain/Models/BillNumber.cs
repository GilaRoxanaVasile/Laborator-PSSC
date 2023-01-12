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
    public record BillNumber
    {


        public string Value { get; }

        internal BillNumber(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new Exception($"{value:0.##} is an invalid bill value.");
            }


        }
        public static Option<BillNumber> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<BillNumber>(new(stringValue));
            }
            else
            {
                return None;
            }
        }
        private static bool IsValid(string numericGrade) => numericGrade != "";
    }
}
