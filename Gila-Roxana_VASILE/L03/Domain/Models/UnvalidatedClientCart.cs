﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static L02_PSSC.Domain.Quantity;

namespace L02_PSSC.Domain
{
    public record UnvalidatedClientCart(Guid cartId, string productCode, string quantity);

}