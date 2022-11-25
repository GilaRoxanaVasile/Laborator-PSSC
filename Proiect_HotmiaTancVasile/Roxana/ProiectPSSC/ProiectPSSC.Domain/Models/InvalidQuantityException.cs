using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ProiectPSSC.Domain.Models
{
    [Serializable]
    internal class InvalidQuantityException:Exception
    {
        public InvalidQuantityException() { }
        public InvalidQuantityException(string? message):base(message) { }
        public InvalidQuantityException(string? message, Exception inner) : base(message, inner) { }
        public InvalidQuantityException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
