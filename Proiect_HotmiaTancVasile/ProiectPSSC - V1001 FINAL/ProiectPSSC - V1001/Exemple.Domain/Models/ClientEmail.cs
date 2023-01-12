using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using LanguageExt.ClassInstances.Pred;

namespace Exemple.Domain.Models
{
    //public record ClientEmail
    //{
    //    //public const string ValidPattern = "^\\S+@\\S+\\.\\S+$";
    //    //private static readonly Regex PatternRegex = new(ValidPattern);


    //    public string Value { get; }

    //    internal ClientEmail(string value)
    //    {
    //        if (IsValid(value))
    //        {
    //            Value = value;
    //        }
    //        else
    //        {
    //            throw new Exception($"{value:0.##} is an invalid ClientEmail value.");
    //        }


    //    }
    //    public static Option<ClientEmail> TryParse(string stringValue)
    //    {
    //        if (IsValid(stringValue))
    //        {
    //            return Some<ClientEmail>(new(stringValue));
    //        }
    //        else
    //        {
    //            return None;
    //        }
    //    }
    //    private static bool IsValid(string stringValue) => stringValue!="";
    //}
    public record ClientEmail { 
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
