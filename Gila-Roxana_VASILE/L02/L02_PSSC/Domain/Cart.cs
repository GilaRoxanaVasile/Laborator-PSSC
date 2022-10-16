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
        public record EmptyCart(IReadOnlyCollection<Product> ProductList) : ICart;
        public record UnvalidatedCart(IReadOnlyCollection<Product> ProductList) : ICart;
        public record ValidatedCart(IReadOnlyCollection<Product> ProductList) : ICart;
        public record PayedCart(IReadOnlyCollection<Product> ProductList) : ICart;
    }
}
