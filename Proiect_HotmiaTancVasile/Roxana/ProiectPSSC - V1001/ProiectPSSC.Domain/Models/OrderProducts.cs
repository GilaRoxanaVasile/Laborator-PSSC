using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPSSC.Domain.Models
{
    [AsChoice]
    public static partial class OrderProducts
    {
        public interface IOrderProducts { }
        public record UnvalidatedOrderProducts: IOrderProducts
        {
            public UnvalidatedOrderProducts(IReadOnlyCollection<UnvalidatedClientOrder> productList)
            {
                ProductList = productList;
            }
            public IReadOnlyCollection<UnvalidatedClientOrder> ProductList { get; }
        }

        public record InvalidOrderProducts:IOrderProducts
        {
            internal InvalidOrderProducts(IReadOnlyCollection<UnvalidatedClientOrder> productList, string reason)
            {
                ProductList = productList;
                Reason = reason;
            }
            public IReadOnlyCollection<UnvalidatedClientOrder> ProductList { get; }
            public string Reason { get; }
        }

        public record ValidatedOrderProducts:IOrderProducts
        {
            internal ValidatedOrderProducts(IReadOnlyCollection<ValidatedClientOrder> productList)
            {
                ProductList = productList;
            }
            public IReadOnlyCollection<ValidatedClientOrder> ProductList { get; }
        }

        public record CalculatedOrderProducts:IOrderProducts
        {
            internal CalculatedOrderProducts(IReadOnlyCollection<CalculatedProductPrice> productList, ClientEmail clientEmail)
            {
                ProductList = productList;
                clientEmail1= clientEmail;
            }
            public ClientEmail clientEmail1 { get; }
            public IReadOnlyCollection<CalculatedProductPrice> ProductList { get; }
        }


        public record PlacedOrderProducts : IOrderProducts
        {
            internal PlacedOrderProducts(IReadOnlyCollection<CalculatedProductPrice> productList, ProductPrice totalPrice, string csv, DateTime publishedDate, ClientEmail clientEmail)
            {
                ProductList = productList;
                Price = totalPrice;
                PublishedDate = publishedDate;
                Csv = csv;
                ClientEmail= clientEmail;
            }

            public ClientEmail ClientEmail { get; }
            public IReadOnlyCollection<CalculatedProductPrice> ProductList { get; }
            public ProductPrice Price { get; }
            public DateTime PublishedDate { get; }
            public string Csv { get; }
        }

    }


}
