using Exemple.Domain.Models;
using static Exemple.Domain.Models.ClientBillsPublishedEvent;
using static Exemple.Domain.ClientBillsOperation;
using System;
using static Exemple.Domain.Models.ClientBills;
using LanguageExt;
using System.Threading.Tasks;
using System.Collections.Generic;
using Exemple.Domain.Repositories;
using System.Linq;
using static LanguageExt.Prelude;
using Microsoft.Extensions.Logging;

namespace Exemple.Domain
{
    public class PublishBillWorkflow
    {
        private readonly IClientRepository clientsRepository;
        private readonly IBillsRepository billsRepository;
        private readonly ILogger<PublishBillWorkflow> logger;

        public PublishBillWorkflow(IClientRepository clientsRepository, IBillsRepository billsRepository, ILogger<PublishBillWorkflow> logger)
        {
            this.clientsRepository = clientsRepository;
            this.billsRepository = billsRepository;
            this.logger = logger;
        }

        public async Task<IClientBillsPublishedEvent> ExecuteAsync(PublishBillsCommand command)
        {
            UnvalidatedClientBills unvalidatedBills = new UnvalidatedClientBills(command.InputBills);

            var result = from clients in clientsRepository.TryGetExistingClients(unvalidatedBills.BillList.Select(bill => bill.ClientEmail))
                                          .ToEither(ex => new FailedClientBills(unvalidatedBills.BillList, ex) as IClientBills)
                         from existingBills in billsRepository.TryGetExistingBills()
                                          .ToEither(ex => new FailedClientBills(unvalidatedBills.BillList, ex) as IClientBills)
                         let checkClientExists = (Func<ClientEmail, Option<ClientEmail>>)(client => CheckClientExists(clients, client))
                         from publishedBills in ExecuteWorkflowAsync(unvalidatedBills, existingBills, checkClientExists).ToAsync()
                         from _ in billsRepository.TrySaveBills(publishedBills)
                                          .ToEither(ex => new FailedClientBills(unvalidatedBills.BillList, ex) as IClientBills)
                         select publishedBills;

            return await result.Match(
                    Left: clientBills => GenerateFailedEvent(clientBills) as IClientBillsPublishedEvent,
                    Right: publishedBills => new ClientBillsPublishScucceededEvent(publishedBills.Csv, publishedBills.PublishedDate)
                );
        }

        private async Task<Either<IClientBills, PublishedClientBills>> ExecuteWorkflowAsync(UnvalidatedClientBills unvalidatedBills,
                                                                                          IEnumerable<CalculatedBillNumber> existingBills,
                                                                                          Func<ClientEmail, Option<ClientEmail>> CheckClientExists)
        {

            IClientBills bills = await ValidateClientBills(CheckClientExists, unvalidatedBills);
             bills = CalculateFinalBills(bills);
             bills = MergeBills(bills, existingBills);
            bills = PublishClientBills(bills);

           

            return bills.Match<Either<IClientBills, PublishedClientBills>>(
                  whenUnvalidatedClientBills: unvalidated => Left(unvalidated as IClientBills),
           whenInvalidClientBills: invalidBills => Left(invalidBills as IClientBills),
           whenFailedClientBills: failedBills => Left(failedBills as IClientBills),
           whenValidatedClientBills: validatedBills => Left(validatedBills as IClientBills),
           whenPublishedClientBills: publishedBills => Right(publishedBills),
           whenCalculatedClientBills: calculatedBills => Left(calculatedBills as IClientBills)

                //whenUnvalidatedBills: unvalidatedBill => Left(unvalidatedBill as IClientBills),
                //whenCalculatedExamGrades: calculatedBills => Left(calculatedBills as IClientBills),
                //whenInvalidExamGrades: invalidBills => Left(invalidBills as IClientBills),
                //whenFailedExamGrades: failedBills => Left(failedBills as IClientBills),
                //whenValidatedExamGrades: validatedBills => Left(validatedBills as IClientBills),
                //whenPublishedExamGrades: publishedBills => Right(publishedBills)
            );
        }

        private Option<ClientEmail> CheckClientExists(IEnumerable<ClientEmail> clients, ClientEmail clientEmail)
        {
            if(clients.Any(c=>c == clientEmail))
            {
                return Some(clientEmail);
            }
            else
            {
                return None;
            }
        }

        private ClientBillsPublishFaildEvent GenerateFailedEvent(IClientBills clientBills) =>
            clientBills.Match<ClientBillsPublishFaildEvent>(

           whenUnvalidatedClientBills: unvalidated => new($"Invalid state {nameof(UnvalidatedClientBills)}"),
           whenInvalidClientBills: invalidBills => new(invalidBills.Reason),
           whenFailedClientBills: failedBills =>
           {
               logger.LogError(failedBills.Exception, failedBills.Exception.Message);
               return new(failedBills.Exception.Message);
           },
           whenValidatedClientBills: validatedBills => new($"Invalid state {nameof(ValidatedClientBills)}"),
           whenCalculatedClientBills: calculatedBills => new($"Invalid state {nameof(CalculatedClientBills)}"),
           whenPublishedClientBills: publishedBills => new($"Invalid state {nameof(PublishedClientBills)}")
           
           );

        //whenUnvalidatedClientBills: unvalidatedClientBills => new($"Invalid state {nameof(UnvalidatedClientBills)}"),
        //        whenInvalidExamGrades: invalidClientBills => new(invalidExamGrades.Reason),
        //        whenValidatedExamGrades: validatedClientBills => new($"Invalid state {nameof(ValidatedClientBills)}"),
        //        whenFailedExamGrades: failedClientBills =>
        //        {
        //            logger.LogError(failedClientBills.Exception, failedClientBills.Exception.Message);
        //            return new(failedClientBills.Exception.Message);
        //        },
        //        whenCalculatedExamGrades: calculatedClientBills => new($"Invalid state {nameof(CalculatedClientBills)}"),
        //        whenPublishedExamGrades: publishedClientBills => new($"Invalid state {nameof(PublishedClientBills)}"));
    }
}
