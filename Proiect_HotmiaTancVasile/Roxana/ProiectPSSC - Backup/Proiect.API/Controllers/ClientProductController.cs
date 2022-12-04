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

namespace ProiectPSSC.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientProductController:ControllerBase
    {
        private ILogger<ClientProductController> logger;

        public ClientProductController(ILogger<ClientProductController> logger)
        {
            this.logger = logger;
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
            return result.Match<IActionResult>(
                whenOrderPlacedFailedEvent: failedEvent => StatusCode(StatusCodes.Status500InternalServerError, failedEvent.Reason),
                whenOrderPlacedSuccededEvent: succesEvent => Ok()
                );
        }

        private static UnvalidatedClientOrder MapInputClientOrderToUnvalidatedOrder(InputClientProduct inputClientProduct) =>
            new UnvalidatedClientOrder(
                ClientEmail: inputClientProduct.CMail,
                ProductCode: inputClientProduct.PCode,
                Quantity: inputClientProduct.Qunatity,
                productPrice: inputClientProduct.Qunatity //help aici
                );
    }
}
