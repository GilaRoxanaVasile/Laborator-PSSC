using Exemple.Domain;
using Exemple.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using Example.Api.Models;
using Exemple.Domain.Models;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Example.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillsController : ControllerBase
    {
        private ILogger<BillsController> logger;
        private readonly PublishBillWorkflow publishBillWorkflow;
        private readonly IHttpClientFactory _httpClientFactory;

        public BillsController(ILogger<BillsController> logger, PublishBillWorkflow publishBillWorkflow, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.publishBillWorkflow = publishBillWorkflow;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("getAllBills")]
        public async Task<IActionResult> GetAllBills([FromServices] IBillsRepository billsRepository) =>
            await billsRepository.TryGetExistingBills().Match(
               Succ: GetAllBillsHandleSuccess,
               Fail: GetAllBillsHandleError
            );

        private ObjectResult GetAllBillsHandleError(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return base.StatusCode(StatusCodes.Status500InternalServerError, "UnexpectedError");
        }

        private OkObjectResult GetAllBillsHandleSuccess(List<Exemple.Domain.Models.CalculatedBillNumber> bills) =>
        Ok(bills.Select(bill => new
        {
            ClientEmail = bill.ClientEmail.Value,
            bill.BillAddress,
            bill.BillNumber
        }));

        [HttpPost]
        public async Task<IActionResult> PublishGrades([FromBody] InputBills[] bills)
        {
            var unvalidatedGrades = bills.Select(MapInputBillToUnvalidatedBill)
                                          .ToList()
                                          .AsReadOnly();
            PublishBillsCommand command = new(unvalidatedGrades);
            var result = await publishBillWorkflow.ExecuteAsync(command);

            return result.Match<IActionResult>(
                whenClientBillsPublishFaildEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
                whenClientBillsPublishScucceededEvent: successEvent => Ok()
                //whenExamGradesPublishFaildEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
                //whenExamGradesPublishScucceededEvent: successEvent => Ok()
            );

            //return await result.MatchAsync(
            //    whenExamGradesPublishFaildEvent: HandleFailure,
            //    whenExamGradesPublishScucceededEvent: HandleSuccess
            //);
        }
        //----------------------------------------------------------------------------------------//
        //public async Task<IActionResult> PublishBills([FromBody] InputBills[] bills)
        //{
        //    var unvalidatedBills = bills.Select(MapInputBillToUnvalidatedBill)
        //                                  .ToList()
        //                                  .AsReadOnly();
        //    PublishBillsCommand command = new(unvalidatedBills);
        //    var result = await publishBillWorkflow.ExecuteAsync(command);

        //    return result.Match<IActionResult>(
        //        whenClientBillsPublishFaildEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
        //        whenClientBillsPublishScucceededEvent: successEvent => Ok()
        //    );

        //return await result.MatchAsync(
        //    whenExamGradesPublishFaildEvent: HandleFailure,
        //    whenExamGradesPublishScucceededEvent: HandleSuccess
        //);
        //}




        //private static UnvalidatedStudentGrade MapInputGradeToUnvalidatedGrade(InputGrade grade) => new UnvalidatedStudentGrade(
        //    StudentRegistrationNumber: grade.RegistrationNumber,
        //    ExamGrade: grade.Exam,
        //    ActivityGrade: grade.Activity,
        //    ClientEmail: grade.ClientEmail,
        //    BillAddress: grade.BillAddress,
        //    BillNumber: grade.BillNumber);

        private static UnvalidatedBill MapInputBillToUnvalidatedBill(InputBills bill) => new UnvalidatedBill(
            ClientEmail: bill.ClientEmail,
            BillAddress: bill.BillAddress,
            BillNumber: bill.BillNumber);
    }
}

