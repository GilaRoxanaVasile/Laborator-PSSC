using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static LanguageExt.Prelude;

namespace ProiectPSSC.Domain.Models
{
    public record ClientEmail
    {
        public const string Pattern = "^\\S+@\\S+\\.\\S+$";
        private static readonly Regex ValidPattern = new Regex(Pattern, RegexOptions.Compiled);
        //private static readonly Regex ValidPattern = new("^\\S+@\\S+\\.\\S+$");

        public string Value { get; }

        public ClientEmail(string value)
        {

            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new Exception($"{value} is invalid");
            }
        }

        public static bool IsValid(string stringValue) => ValidPattern.IsMatch(stringValue);

        public static bool IsValidEmail(string emailString, out ClientEmail clientMail)
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

        public static Option<ClientEmail> TryParseClientEmail(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<ClientEmail>(new(stringValue));
            }
            else
            {
                return None;
            }
        }
    }
}
