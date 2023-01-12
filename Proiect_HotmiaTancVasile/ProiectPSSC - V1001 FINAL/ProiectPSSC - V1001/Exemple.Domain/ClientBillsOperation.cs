using Exemple.Domain.Models;
using static LanguageExt.Prelude;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Exemple.Domain.Models.ClientBills;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    public static class ClientBillsOperation
    {
        public static Task<IClientBills> ValidateClientBills(Func<ClientEmail, Option<ClientEmail>> checkClientExists, UnvalidatedClientBills clientBill) =>
            clientBill.BillList
                      .Select(ValidateClientBill(checkClientExists))
                      .Aggregate(CreateEmptyValatedGradesList().ToAsync(), ReduceValidBills)
                      .MatchAsync(
                            Right: validatedBills => new ValidatedClientBills(validatedBills),
                            LeftAsync: errorMessage => Task.FromResult((IClientBills)new InvalidClientBills(clientBill.BillList, errorMessage))
                      );

        private static Func<UnvalidatedBill, EitherAsync<string, ValidatedClientBill>> ValidateClientBill(Func<ClientEmail, Option<ClientEmail>> checkClientExists) =>
            unvalidatedClientBill => ValidateClientBill(checkClientExists, unvalidatedClientBill);

        private static EitherAsync<string, ValidatedClientBill> ValidateClientBill(Func<ClientEmail, Option<ClientEmail>> checkClientExists, UnvalidatedBill unvalidatedBill) =>

            from clientEmail in ClientEmail.TryParseClientEmail(unvalidatedBill.ClientEmail)
                                   .ToEitherAsync($"Invalid activity bill ({unvalidatedBill.ClientEmail})")
            from billAddress in BillAddress.TryParse(unvalidatedBill.BillAddress)
                               .ToEitherAsync($"Invalid activity bill ({unvalidatedBill.ClientEmail})")
            from billNumber in BillNumber.TryParse(unvalidatedBill.BillNumber)
                           .ToEitherAsync($"Invalid activity bill ({unvalidatedBill.ClientEmail})")
            from clientExists in checkClientExists(clientEmail)
                                   .ToEitherAsync($"Student {clientEmail.Value} does not exist.")
            select new ValidatedClientBill(clientEmail, billAddress, billNumber);

        private static Either<string, List<ValidatedClientBill>> CreateEmptyValatedGradesList() =>
            Right(new List<ValidatedClientBill>());

        private static EitherAsync<string, List<ValidatedClientBill>> ReduceValidBills(EitherAsync<string, List<ValidatedClientBill>> acc, EitherAsync<string, ValidatedClientBill> next) =>
            from list in acc
            from nextBill in next
            select list.AppendValidGrade(nextBill);

        private static List<ValidatedClientBill> AppendValidGrade(this List<ValidatedClientBill> list, ValidatedClientBill validBill)
        {
            list.Add(validBill);
            return list;
        }

        public static IClientBills CalculateFinalBills(IClientBills clientBill) => clientBill.Match(
              whenUnvalidatedClientBills: unvalidated => unvalidated,
           whenInvalidClientBills: invalid => invalid,
           whenFailedClientBills: failed => failed,
           whenPublishedClientBills: published => published,
           whenCalculatedClientBills: calculated => calculated,
            whenValidatedClientBills: CalculateFinalBill
            //whenUnvalidatedBills: unvalidatedClientBill => unvalidatedClientBill,
            //whenInvalidExamGrades: invalidExam => invalidExam,
            //whenFailedExamGrades: failedExam => failedExam,
            //whenCalculatedExamGrades: calculatedExam => calculatedExam,
            //whenPublishedExamGrades: publishedExam => publishedExam,
            //whenValidatedExamGrades: CalculateFinalBill
        );

        private static IClientBills CalculateFinalBill(ValidatedClientBills validClientBills) =>
            new CalculatedClientBills(validClientBills.BillList
                                                    .Select(CalculateClientFinalBill)
                                                    .ToList()
                                                    .AsReadOnly());

        private static CalculatedBillNumber CalculateClientFinalBill(ValidatedClientBill validBill) =>
            new CalculatedBillNumber(validBill.clientEmail,
                                      //validBill.ExamGrade,
                                      //validBill.ActivityGrade,

                                      //validBill.ExamGrade + validBill.ActivityGrade,

                                      validBill.billAddress,
                                      validBill.billNumber);

        public static IClientBills MergeBills(IClientBills clientBill, IEnumerable<CalculatedBillNumber> existingBills) => clientBill.Match(
           whenUnvalidatedClientBills: unvalidated => unvalidated,
           whenInvalidClientBills: invalid => invalid,
           whenFailedClientBills: failed => failed,
           whenValidatedClientBills: validated => validated,
           whenPublishedClientBills: published => published,
           whenCalculatedClientBills: calculated => MergeBills(calculated.BillList, existingBills));

        //public static IClientBills MergeBills(IClientBills clientBill, IEnumerable<CalculatedBillNumber> existingBills) => clientBill.Match(
        //    whenUnvalidatedClientBills: unvalidaTedExam => unvalidaTedExam,
        //    whenInvalidExamGrades: invalidExam => invalidExam,
        //    whenFailedExamGrades: failedExam => failedExam,
        //    whenValidatedExamGrades: validatedExam => validatedExam,
        //    whenPublishedExamGrades: publishedExam => publishedExam,
        //    whenCalculatedExamGrades: calculatedExam => MergeBills(calculatedExam.BillList, existingBills));

        private static CalculatedClientBills MergeBills(IEnumerable<CalculatedBillNumber> newList, IEnumerable<CalculatedBillNumber> existingList)
        {
            var updatedAndNewBills = newList.Select(bill => bill with { BillId = existingList.FirstOrDefault(g => g.ClientEmail == bill.ClientEmail)?.BillId ?? 0, IsUpdated = true });
            var oldBills = existingList.Where(bill => !newList.Any(g => g.ClientEmail == bill.ClientEmail));
            var allBills = updatedAndNewBills.Union(oldBills)
                                               .ToList()
                                               .AsReadOnly();
            return new CalculatedClientBills(allBills);
        }

        public static IClientBills PublishClientBills(IClientBills clientBill) => clientBill.Match(
      whenUnvalidatedClientBills: unvalidated => unvalidated,
           whenInvalidClientBills: invalid => invalid,
           whenFailedClientBills: failed => failed,
           whenValidatedClientBills: validated => validated,
           whenPublishedClientBills: published => published,
           whenCalculatedClientBills: GenerateExport);

        private static IClientBills GenerateExport(CalculatedClientBills calculatedBill) =>
            new PublishedClientBills(calculatedBill.BillList,
                                    calculatedBill.BillList.Aggregate(new StringBuilder(), CreateCsvLine).ToString(),
                                    DateTime.Now);

        private static StringBuilder CreateCsvLine(StringBuilder export, CalculatedBillNumber bill) =>
        //export.AppendLine($"{bill.ClientEmail.Value}, {bill.ExamGrade}, {bill.ActivityGrade}, {bill.FinalGrade}");
            export.AppendLine($"{bill.ClientEmail.Value}, {bill.BillNumber}");
    }
}
