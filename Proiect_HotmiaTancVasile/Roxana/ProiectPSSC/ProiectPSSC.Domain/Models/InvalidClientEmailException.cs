using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Models
{
    [Serializable]
    internal class InvalidClientEmailException : Exception
    {
        public InvalidClientEmailException()
        {
        }

        public InvalidClientEmailException(string? message) : base(message)
        {
        }

        public InvalidClientEmailException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidClientEmailException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
