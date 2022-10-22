using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace L02_PSSC.Domain
{
    public record ClientMail
    {
        private static readonly Regex ValidPattern = new("^\\S+@\\S+\\.\\S+$");
        //private static readonly Regex ValidPattrern = new("^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?$") 

        public string Value { get; }

        public ClientMail(string value)
        {
            
            if(IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new Exception($"{value} is invalid");
            }
        }

        public static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public static bool IsValidEmail(string emailString, out ClientMail clientMail)
        {
            bool isValid = false;
            clientMail = null;
            try
            {
                var mail = new System.Net.Mail.MailAddress(emailString);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
