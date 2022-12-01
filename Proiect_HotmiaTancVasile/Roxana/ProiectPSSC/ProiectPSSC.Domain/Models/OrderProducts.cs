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
            internal CalculatedOrderProducts(IReadOnlyCollection<CalculatedProductPrice> productList)
            {
                ProductList = productList;
            }
            public IReadOnlyCollection<CalculatedProductPrice> ProductList { get; }
        }


        public record PlacedOrderProducts : IOrderProducts
        {
            internal PlacedOrderProducts(ClientEmail client, IReadOnlyCollection<CalculatedProductPrice> productList, ProductPrice totalPrice, string csv, DateTime publishedDate)
            {
                Client = client;
                ProductList = productList;
                Price = totalPrice;
                PublishedDate = publishedDate;
                Csv = csv;
            }
            public ClientEmail Client { get; }
            public IReadOnlyCollection<CalculatedProductPrice> ProductList { get; }
            public ProductPrice Price { get; }
            public DateTime PublishedDate { get; }
            public string Csv { get; }
        }

        /*

        public record PlacedOrderProducts:IOrderProducts
        {
            internal PlacedOrderProducts(IReadOnlyCollection<CalculatedOrderTotalPayment> productList, string csv, DateTime publishedDate)
            {
                ProductList = productList;
                PublishedDate = publishedDate;
                Csv = csv;
            }
            public IReadOnlyCollection<CalculatedOrderTotalPayment> ProductList { get; }
            public DateTime PublishedDate { get; }
            public string Csv { get; }
        }
        */
    }


}
