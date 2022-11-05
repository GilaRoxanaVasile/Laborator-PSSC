using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace L02_PSSC.Domain
{
    public record Address
    {
        public static readonly Regex ValidPattern = new("^[a-zA-Z]");
        public string address { get; }
        public Address(string addr) 
        { 
            address = addr; 
        }
        public static bool TryParseAddress(string addrString, out Address address)
        {
            bool isValid = false;
            address = null;
            if (IsValid(addrString))
            {
                address = new Address(addrString);
                isValid=true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }
        public static bool IsValid(string addrValue) => ValidPattern.IsMatch(addrValue);
        public override string ToString() 
        {
            return address; 
        }
    }
}
