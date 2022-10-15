using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L01_PSSC.Domain
{
    public record Contact2(Contact Contact, List<ListOfProducs> ListOfProducs);

}