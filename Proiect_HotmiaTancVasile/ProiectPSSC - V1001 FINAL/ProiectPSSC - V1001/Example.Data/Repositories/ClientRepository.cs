using Exemple.Domain.Models;
using Exemple.Domain.Repositories;
using LanguageExt;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Example.Data.Repositories
{
    public class ClientRepository: IClientRepository
    {
        private readonly BillsContext billsContext;

        public ClientRepository(BillsContext billsContext)
        {
            this.billsContext = billsContext;  
        }

        public TryAsync<List<ClientEmail>> TryGetExistingClients(IEnumerable<string> clientsToCheck) => async () =>
        {
            var clients = await billsContext.Clients
                                              .Where(client => clientsToCheck.Contains(client.ClientEmail))
                                              .AsNoTracking()
                                              .ToListAsync();
            return clients.Select(client => new ClientEmail(client.ClientEmail))
                           .ToList();
        };
    }
}
