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
    /*
    REATE TABLE [dbo].[Client](
	[ClientId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](20) NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[PhoneNumber] [varchar](15) NOT NULL,
	[CardDetails] [varchar](50),  
     */
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
                                .Where(client => clientsToCheck.Contains(client.ClientEmail))
                                .AsNoTracking()
                                .ToListAsync();
            return clients.Select(client => new ClientEmail(client.ClientEmail))
                            .ToList();
        };
    }
}
