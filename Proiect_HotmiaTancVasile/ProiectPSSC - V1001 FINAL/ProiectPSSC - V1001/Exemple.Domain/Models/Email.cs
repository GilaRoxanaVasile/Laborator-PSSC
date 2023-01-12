using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
namespace Exemple.Domain.Models
{
    public record Email
    {
        //public const string ValidPattern = "^\\S+@\\S+\\.\\S+$";
        //private static readonly Regex PatternRegex = new(ValidPattern);


        public string Value { get; }

        internal Email(string value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                throw new Exception($"{value:0.##} is an invalid email value.");
            }
            
           
        }
        public static Option<Email> TryParse(string stringValue)
        {
            if (IsValid(stringValue))
            {
                return Some<Email>(new(stringValue));
            }
            else
            {
                return None;
            }
        }
        private static bool IsValid(string stringValue) => stringValue!="";
    }

    }
