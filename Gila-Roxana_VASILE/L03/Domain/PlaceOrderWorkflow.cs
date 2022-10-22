using System;
using Domain.Models;
using L02_PSSC.Domain;
using static Domain.Models.OrderPlacedEvent;
using static Domain.CartOperation;
using static L02_PSSC.Domain.Cart;
using System.Net.Http.Headers;

namespace Domain.Models
{
    public class PlaceOrderWorkflow
    {
        public IOrderPlacedEvent Execute(Client client, Guid cartID, PlaceOrderCommand command, Func<ProductCode,bool> checkProductExists)
        {
            UnvalidatedCart unvalidatedClientCart = new UnvalidatedCart(cartID, command.InputProducts);
            ICart cart = ValidateCartProducts(client, checkProductExists, unvalidatedClientCart);
            string csv = "ok";
            return cart.Match(
                whenEmptyCart: emptyCart => new OrderPlacingFailedEvent("invalid") as IOrderPlacedEvent,
                whenInvalidatedCart: invalidCart => new OrderPlacingFailedEvent(invalidCart.reason),
                whenValidatedCart: validatedCart => new OrderPlacingFailedEvent("invalid"),
                whenUnvalidatedCart: unvalidatedCart => new OrderPlacingFailedEvent("invalid"),
                whenPayedCart: payedCart => new OrderPlacingSucceded(csv, DateTime.Now)
                );
        }
    }
}
