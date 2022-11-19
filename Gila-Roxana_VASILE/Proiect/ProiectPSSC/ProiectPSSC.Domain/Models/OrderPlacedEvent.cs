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
            internal OrderPlacedSuccededEvent(string csv, DateTime publishedDate)
            {
                Csv = csv;
                PublishedDate = publishedDate;
            }
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
