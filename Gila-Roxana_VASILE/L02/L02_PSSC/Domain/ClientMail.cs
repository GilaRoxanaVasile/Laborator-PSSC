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

        public string Value { get; }

        public ClientMail(string value)
        {
            if(IsValidEmail(value))
            {
                Value = value;
            }
            else
            {
                throw new Exception($"{value} is invalid");
            }
        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
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
