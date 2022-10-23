using CSharp.Choices;
using Domain.Models;
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
        public record UnvalidatedCart(Guid IdCart, IReadOnlyCollection<UnvalidatedClientCart> ProductList) : ICart
        {
            public IReadOnlyCollection<UnvalidatedClientCart> ProductList { get; }
            public Guid IdCart { get; }
        }
        public record InvalidatedCart(IReadOnlyCollection<UnvalidatedClientCart> productList, string reason): ICart
        {
            public IReadOnlyCollection<UnvalidatedClientCart> ProductList { get; }
            public string Reason { get; }
        }
        public record ValidatedCart: ICart
        {
            internal ValidatedCart(IReadOnlyCollection<ValidatedClientCart> productList)
            {
                productList = ProductList;
            }
            public IReadOnlyCollection<ValidatedClientCart> ProductList { get; }
        }
        
        public record PayedCart : ICart
        {
            internal PayedCart(IReadOnlyCollection<CalculatedTotalPrice> productList, string csv, DateTime dayOfPayment)
            {
                ProductList = productList;
                Csv = csv;
                PaymentDate = dayOfPayment;
            }
            public IReadOnlyCollection<CalculatedTotalPrice> ProductList { get; }
            public string Csv { get; }
            public DateTime PaymentDate { get; }

        }
        public record CalculatedCartPrice(IReadOnlyCollection<CalculatedTotalPrice> productList):ICart
        {

            public decimal GetTotal()
            {
                decimal total = 0;
                foreach(var product in ProductList)
                {
                    total += product.totalPrice;
                }
                return total;
            }
            public IReadOnlyCollection<CalculatedTotalPrice> ProductList { get; }

        }
    }
}
