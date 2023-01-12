using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ProiectPSSC.Domain.Models
{
    [Serializable]
    internal class InvalidPriceException : Exception
    {
        public InvalidPriceException()
        {
        }

        public InvalidPriceException(string? message) : base(message)
        {
        }

        public InvalidPriceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidPriceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
