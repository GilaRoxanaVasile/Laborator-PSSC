using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Models
{
    [AsChoice]
    public static partial class OrderPlacedEvent
    {
        public interface IOrderPlacedEvent { }
        public record OrderPlacedSuccededEvent:IOrderPlacedEvent
        {
            internal OrderPlacedSuccededEvent(ClientEmail clientEmail, ProductPrice price, string csv, DateTime publishedDate)
            {
                Price = price;
                Csv = csv;
                PublishedDate = publishedDate;
                clientEmail1 = clientEmail;
            }
            public ClientEmail clientEmail1 { get; }
            public ProductPrice Price { get; }
            public string Csv { get; }
            public DateTime PublishedDate { get; }
        }
        public record OrderPlacedFailedEvent:IOrderPlacedEvent
        {
            internal OrderPlacedFailedEvent(string reason)
            {
                Reason = reason;
            }
            public string Reason { get; }
        }

    }
}
