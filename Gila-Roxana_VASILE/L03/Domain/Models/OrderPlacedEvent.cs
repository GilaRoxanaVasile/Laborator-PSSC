using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [AsChoice]
    public static partial class OrderPlacedEvent
    {
        public interface IOrderPlacedEvent { }
        public record OrderPlacingSucceded:IOrderPlacedEvent
        {
            public string CSV { get; } //csv? as in exel kind of thing .csv file?
            public DateTime PublishedDate { get; }
            internal OrderPlacingSucceded(string csv, DateTime publishedDate)
            {
                CSV = csv;
                PublishedDate = publishedDate;
            }
        }
        public record OrderPlacingFailedEvent:IOrderPlacedEvent
        {
            public string Reason { get; }
            internal OrderPlacingFailedEvent(string reason)
            {
                Reason = reason;
            }
        }
    }
}
