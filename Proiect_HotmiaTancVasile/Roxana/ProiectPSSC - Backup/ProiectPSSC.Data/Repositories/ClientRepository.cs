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
        private readonly OrderContext _orderContext;
        public ClientRepository(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public TryAsync<List<ClientEmail>> TryGetExistingClients(IEnumerable<string> clientsToCheck) => async() =>
        {
            var clients = await _orderContext.Clients
                                .Where(client => clientsToCheck.Contains(client.Email))
                                .AsNoTracking()
                                .ToListAsync();
            return clients.Select(client => new ClientEmail(client.Email))
                            .ToList();
        };
    }
}
