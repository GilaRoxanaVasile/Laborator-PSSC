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
        private static readonly Regex ValidPattrern = new("^x[0-9]{3}x$");

        public string Value { get; }

        public ProductCode(string value)
        {
            if (ValidPattrern.IsMatch(value))
            {
                Value = value;
            }
            else
            {
                throw new Exception($"{value} is invalid");
            }
        }
        public override string ToString()
        {
            return Value;
        }
    }
}
