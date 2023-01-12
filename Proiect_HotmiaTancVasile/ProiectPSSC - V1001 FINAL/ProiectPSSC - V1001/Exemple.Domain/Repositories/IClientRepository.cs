using Exemple.Domain.Models;
using LanguageExt;
using System.Collections.Generic;
using static Exemple.Domain.Models.ClientBills;

namespace Exemple.Domain.Repositories
{
    public interface IClientRepository
    {
        TryAsync<List<ClientEmail>> TryGetExistingClients(IEnumerable<string> clientsToCheck);
    
    }
}
