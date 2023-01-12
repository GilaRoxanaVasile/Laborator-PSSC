using Exemple.Domain.Models;
using LanguageExt;
using System.Collections.Generic;
using static Exemple.Domain.Models.ClientBills;

namespace Exemple.Domain.Repositories
{
    public interface IBillsRepository
    {
        TryAsync<List<CalculatedBillNumber>> TryGetExistingBills();

        TryAsync<Unit> TrySaveBills(PublishedClientBills bills);
    }
}
