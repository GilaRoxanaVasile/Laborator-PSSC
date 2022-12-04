using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Models
{
    [Serializable]
    internal class InvalidProductCodeException : Exception
    {
        public InvalidProductCodeException()
        {
        }

        public InvalidProductCodeException(string? message) : base(message)
        {
        }

        public InvalidProductCodeException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidProductCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
