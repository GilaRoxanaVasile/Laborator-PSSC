using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class ClientBills
    {
        public interface IClientBills { }

        public record UnvalidatedClientBills : IClientBills
        {
            public UnvalidatedClientBills(IReadOnlyCollection<UnvalidatedBill> billList)
            {
                BillList = billList;
            }

            public IReadOnlyCollection<UnvalidatedBill> BillList { get; }
        }

        public record InvalidClientBills : IClientBills
        {
            internal InvalidClientBills(IReadOnlyCollection<UnvalidatedBill> billList, string reason)
            {
                BillList = billList;
                Reason = reason;
            }

            public IReadOnlyCollection<UnvalidatedBill> BillList { get; }
            public string Reason { get; }
        }

        public record FailedClientBills : IClientBills
        {
            internal FailedClientBills(IReadOnlyCollection<UnvalidatedBill> billList, Exception exception)
            {
                BillList = billList;
                Exception = exception;
            }

            public IReadOnlyCollection<UnvalidatedBill> BillList { get; }
            public Exception Exception { get; }
        }

        public record ValidatedClientBills : IClientBills
        {
            internal ValidatedClientBills(IReadOnlyCollection<ValidatedClientBill> billsList)
            {
                BillList = billsList;
            }

            public IReadOnlyCollection<ValidatedClientBill> BillList { get; }
        }

        public record CalculatedClientBills : IClientBills
        {
            internal CalculatedClientBills(IReadOnlyCollection<CalculatedBillNumber> billsList)
            {
                BillList = billsList;
            }

            public IReadOnlyCollection<CalculatedBillNumber> BillList { get; }
        }

        public record PublishedClientBills : IClientBills
        {
            internal PublishedClientBills(IReadOnlyCollection<CalculatedBillNumber> billsList, string csv, DateTime publishedDate)
            {
                BillList = billsList;
                PublishedDate = publishedDate;
                Csv = csv;
            }

            public IReadOnlyCollection<CalculatedBillNumber> BillList { get; }
            public DateTime PublishedDate { get; }
            public string Csv { get; }
        }
    }
}
