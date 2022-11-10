using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;

namespace DataModels
{
    public class ProductsContext: DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options):base(options)
        {

        }

        public DbSet<OrderLineDto> OrderLines { get; set; }
        public DbSet<OrderHeaderDto> OrderHeaders { get;set; }
        public DbSet<ProductDto> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderLineDto>().ToTable("OrderLine").HasKey(s => s.OrderLineId);
            modelBuilder.Entity<ProductDto>().ToTable("Product").HasKey(s => s.ProductId);
            modelBuilder.Entity<OrderHeaderDto>().ToTable("OrderHeader").HasKey(s => s.OrderId);
        }

    }
}
