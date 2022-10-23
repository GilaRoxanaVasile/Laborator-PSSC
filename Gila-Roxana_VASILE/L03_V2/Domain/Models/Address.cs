using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L02_PSSC.Domain
{
    public record Address
    {
        public string address;
        public Address(string addr) { address = addr; }
        public override string ToString() { return address; }
    }
}
