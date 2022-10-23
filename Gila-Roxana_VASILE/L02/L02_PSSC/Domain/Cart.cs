using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L02_PSSC.Domain
{
    [AsChoice]
    public static partial class Cart
    {
        public interface ICart { }
        public record EmptyCart(Guid IdCart) : ICart;
        public record InvalidatedCart(Guid IdCart, Client ? client, IReadOnlyCollection<UnvalidatedClientCart> ProductList, string reason) : ICart;
        public record ValidatedCart(Guid IdCart, Client client, IReadOnlyCollection<ValidatedClientCart> ProductList) : ICart;
        public record PayedCart(Guid IdCart, Client client, IReadOnlyCollection<ValidatedClientCart> ProductList, DateTime timeOfPayment) : ICart;
    }
}
