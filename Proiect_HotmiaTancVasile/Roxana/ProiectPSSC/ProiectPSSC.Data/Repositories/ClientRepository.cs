using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;
using ProiectPSSC.Domain.Models;
using ProiectPSSC.Domain.Repositories;
using LanguageExt;

namespace ProiectPSSC.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        public TryAsync<List<ClientEmail>> TryGetExistingClients(IEnumerable<string> clientsToCheck)
        {
            throw new NotImplementedException();
        }
    }
}
