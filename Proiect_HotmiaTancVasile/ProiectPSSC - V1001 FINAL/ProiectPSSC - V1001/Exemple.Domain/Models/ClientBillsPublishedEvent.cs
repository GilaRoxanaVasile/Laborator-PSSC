using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class ClientBillsPublishedEvent
    {
        public interface IClientBillsPublishedEvent { }

        public record ClientBillsPublishScucceededEvent : IClientBillsPublishedEvent
        {
            public string Csv{ get;}
            public DateTime PublishedDate { get; }

            internal ClientBillsPublishScucceededEvent(string csv, DateTime publishedDate)
            {
                Csv = csv;
                PublishedDate = publishedDate;
            }
        }

        public record ClientBillsPublishFaildEvent : IClientBillsPublishedEvent
        {
            public string Reason { get; }

            internal ClientBillsPublishFaildEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
