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
    public class GradesController : ControllerBase
    {
        private ILogger<GradesController> logger;
        private readonly PublishGradeWorkflow publishGradeWorkflow;
        private readonly IHttpClientFactory _httpClientFactory;

        public GradesController(ILogger<GradesController> logger, PublishGradeWorkflow publishGradeWorkflow, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.publishGradeWorkflow = publishGradeWorkflow;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("getAllGrades")]
        public async Task<IActionResult> GetAllGrades([FromServices] IGradesRepository gradesRepository) =>
            await gradesRepository.TryGetExistingGrades().Match(
               Succ: GetAllGradesHandleSuccess,
               Fail: GetAllGradesHandleError
            );

        private ObjectResult GetAllGradesHandleError(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return base.StatusCode(StatusCodes.Status500InternalServerError, "UnexpectedError");
        }

        private OkObjectResult GetAllGradesHandleSuccess(List<Exemple.Domain.Models.CalculatedSudentGrade> grades) =>
        Ok(grades.Select(grade => new
        {
            StudentRegistrationNumber = grade.StudentRegistrationNumber.Value,
            grade.ExamGrade,
            grade.ActivityGrade,
            grade.FinalGrade
        }));

        [HttpPost]
        public async Task<IActionResult> PublishGrades([FromBody]InputGrade[] grades)
        {
            var unvalidatedGrades = grades.Select(MapInputGradeToUnvalidatedGrade)
                                          .ToList()
                                          .AsReadOnly();
            PublishGradesCommand command = new(unvalidatedGrades);
            var result = await publishGradeWorkflow.ExecuteAsync(command);

            //return result.Match<IActionResult>(
            //    whenExamGradesPublishFaildEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
            //    whenExamGradesPublishScucceededEvent: successEvent => Ok()
            //);

            return await result.MatchAsync(
                whenExamGradesPublishFaildEvent: HandleFailure,
                whenExamGradesPublishScucceededEvent: HandleSuccess
            );
        }

        private Task<IActionResult> HandleFailure(ExamGradesPublishedEvent.ExamGradesPublishFaildEvent failedEvent)
        {
            return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason));
        } 

        private async Task<IActionResult> HandleSuccess(ExamGradesPublishedEvent.ExamGradesPublishScucceededEvent successEvent)
        {
            var w1 = TriggerReportGeneration(successEvent);
            var w2 = TriggerScholarshipCalculation(successEvent);
            await Task.WhenAll(w1, w2);
            return Ok();
        }

        private async Task<Boolean> TriggerReportGeneration(ExamGradesPublishedEvent.ExamGradesPublishScucceededEvent successEvent)
        {
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "https://localhost:7286/report/semester-report")
            {
                Content = new StringContent(JsonConvert.SerializeObject(successEvent), Encoding.UTF8, "application/json")
            };
           var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(httpRequestMessage);
            return true;
        }

        private async Task<Boolean> TriggerScholarshipCalculation(ExamGradesPublishedEvent.ExamGradesPublishScucceededEvent successEvent)
        {
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Post,
            "https://localhost:7286/report/scholarship")
            {
                Content = new StringContent(JsonConvert.SerializeObject(successEvent), Encoding.UTF8, "application/json")
            };
            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(httpRequestMessage);
            return true;
        }

        private static UnvalidatedStudentGrade MapInputGradeToUnvalidatedGrade(InputGrade grade) => new UnvalidatedStudentGrade(
            StudentRegistrationNumber: grade.RegistrationNumber,
            ExamGrade: grade.Exam,
            ActivityGrade: grade.Activity);
    }
}
