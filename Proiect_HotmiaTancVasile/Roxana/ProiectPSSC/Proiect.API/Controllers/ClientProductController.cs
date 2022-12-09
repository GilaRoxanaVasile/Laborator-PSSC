using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using ProiectPSSC.Domain;
using ProiectPSSC.Domain.Repositories;
using ProiectPSSC.Api.Models;
using ProiectPSSC.Domain.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace ProiectPSSC.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientProductController:ControllerBase
    {
        private ILogger<ClientProductController> logger;
        private readonly PlaceOrderWorkflow placeOrderWorkflow;
        private readonly IHttpClientFactory httpClientFactory;

        public ClientProductController(ILogger<ClientProductController> logger, PlaceOrderWorkflow placeOrderWorkflow, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.placeOrderWorkflow = placeOrderWorkflow;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromServices] IOrderHeaderRepository productRepository) =>
                await productRepository.TryGetExistingClientOrders().Match(
                    Succ: GetAllProductsHandleSucces,
                    Fail: GetAllProductsHandleError
                    );
        private OkObjectResult GetAllProductsHandleSucces(List<ProiectPSSC.Domain.Models.CalculatedOrderTotalPrice> order) =>
            Ok(order.Select(product => new
            {
                ClientEmail = product.clientEmail.Value,
                product.totalPrice,
                
            }));
        private ObjectResult GetAllProductsHandleError(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return base.StatusCode(StatusCodes.Status500InternalServerError, "UnexpectedError");
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromServices] PlaceOrderWorkflow placeOrderWorkflow, [FromBody] InputClientProduct[] inputClientProducts)
        {
            var unvalidatedOrder = inputClientProducts.Select(MapInputClientOrderToUnvalidatedOrder)
                .ToList()
                .AsReadOnly();
            PlaceOrderCommand command = new(unvalidatedOrder);
            var result = await placeOrderWorkflow.EventAsync(command);

            //return result.Match<IActionResult>(
            //    whenOrderPlacedFailedEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
            //    whenOrderPlacedSuccededEvent: succesEvent => Ok(succesEvent)
            //    );

            return await result.MatchAsync(
                whenOrderPlacedFailedEvent: HandleFailure,
                whenOrderPlacedSuccededEvent: HandleSucces
                );
         }

        private Task<IActionResult> HandleFailure(OrderPlacedEvent.OrderPlacedFailedEvent failedEvent)
        {
            return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason));
        }

        private async Task<IActionResult> HandleSucces(OrderPlacedEvent.OrderPlacedSuccededEvent succededEvent)
        {
            var w1 = TriggerBilling(succededEvent);
            var w2 = TriggerShipping(succededEvent);
            await Task.WhenAll(w1, w2);
            return Ok();
        }

        private async Task <Boolean> TriggerBilling(OrderPlacedEvent.OrderPlacedSuccededEvent succededEvent)
        {
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Post, "")
            {
                Content = new StringContent(JsonConvert.SerializeObject(succededEvent), Encoding.UTF8, "application/json")
            };
            var client = httpClientFactory.CreateClient();
            var response = await client.SendAsync(httpRequestMessage);
            return true;
        }

        private async Task<Boolean> TriggerShipping(OrderPlacedEvent.OrderPlacedSuccededEvent succededEvent)
        {
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Post, "")
            {
                Content = new StringContent(JsonConvert.SerializeObject(succededEvent), Encoding.UTF8, "application/json")
            };
            var client = httpClientFactory.CreateClient();
            var response = await client.SendAsync(httpRequestMessage);
            return true;
        }


        private static UnvalidatedClientOrder MapInputClientOrderToUnvalidatedOrder(InputClientProduct inputClientProduct) =>
            new UnvalidatedClientOrder(
                ClientEmail: inputClientProduct.CMail,
                ProductCode: inputClientProduct.PCode,
                Quantity: inputClientProduct.Qunatity
                );
    }
}
