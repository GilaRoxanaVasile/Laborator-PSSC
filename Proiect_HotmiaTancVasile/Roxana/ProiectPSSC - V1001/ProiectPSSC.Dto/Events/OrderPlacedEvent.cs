using ProiectPSSC.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Dto.Events
{
    public record OrderPlacedEvent
    {
        public List<OrderDto> OrderProducts { get; init; }
    }
}