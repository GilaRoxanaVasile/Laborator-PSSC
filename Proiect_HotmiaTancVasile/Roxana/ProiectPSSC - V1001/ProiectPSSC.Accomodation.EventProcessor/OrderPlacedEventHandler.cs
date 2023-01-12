using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ProiectPSSC.Domain.Models;
using ProiectPSSC.Events;
using ProiectPSSC.Events.Models;
using ProiectPSSC.Dto.Events;
//using static ProiectPSSC.Domain.Models.OrderPlacedEvent;

namespace ProiectPSSC.Accomodation.EventProcessor
{
    internal class OrderPlacedEventHandler : AbstractEventHandler<ProiectPSSC.Dto.Events.OrderPlacedEvent>
    {
        public override string[] EventTypes => new string[] { typeof(OrderPlacedEvent).Name };

        protected override Task<EventProcessingResult> OnHandleAsync(OrderPlacedEvent eventData)
        {
            Console.WriteLine(eventData.ToString());
            return Task.FromResult(EventProcessingResult.Completed);
        }
    }
}
