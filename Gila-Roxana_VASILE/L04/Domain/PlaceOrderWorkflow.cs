using System;
using Domain.Models;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using L02_PSSC.Domain;
using static Domain.Models.OrderPlacedEvent;
using static Domain.CartOperation;
using static L02_PSSC.Domain.Cart;
using System.Net.Http.Headers;

namespace Domain.Models
{
    public class PlaceOrderWorkflow
    {
        public async Task <IOrderPlacedEvent> ExecuteAsync(Client client, Guid cartID, PlaceOrderCommand command, Func<ProductCode,TryAsync<bool>> checkProductExists)
        {
            UnvalidatedCart unvalidatedClientCart = new UnvalidatedCart(cartID, command.InputProducts);
            
            ICart cart = await ValidateCartProducts(client, cartID, checkProductExists, unvalidatedClientCart);
            cart = CalculateFinalPrices(cart);
            cart = SendOrder(cart);

            return cart.Match(
                whenEmptyCart: emptyCart => new OrderPlacingFailedEvent("invalid") as IOrderPlacedEvent,
                whenInvalidatedCart: invalidCart => new OrderPlacingFailedEvent(invalidCart.reason),
                whenValidatedCart: validatedCart => new OrderPlacingFailedEvent("invalid"),
                whenUnvalidatedCart: unvalidatedCart => new OrderPlacingFailedEvent("invalid"),
                whenCalculatedCartPrice: calculatedCart => new OrderPlacingFailedEvent("invalid"),
                whenPayedCart: payedCart => new OrderPlacingSucceded(payedCart.Csv, DateTime.Now)
                );
        }
    }
}
