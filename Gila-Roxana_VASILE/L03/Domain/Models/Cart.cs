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
        public record UnvalidatedCart(Guid IdCart, IReadOnlyCollection<UnvalidatedClientCart> ProductList) : ICart;
        public record InvalidatedCart(IReadOnlyCollection<UnvalidatedClientCart> ProductList, string reason) : ICart;
        public record ValidatedCart(IReadOnlyCollection<ValidatedClientCart> ProductList) : ICart;
        public record PayedCart(IReadOnlyCollection<ValidatedClientCart> ProductList, DateTime timeOfPayment) : ICart;
    }
}
