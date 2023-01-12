using Example.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Data
{
    public class BillsContext: DbContext
    {
        public BillsContext(DbContextOptions<BillsContext> options) : base(options)
        {
        }

        public DbSet<BillDto> Bills { get; set; }

        public DbSet<ClientDto> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientDto>().ToTable("Client").HasKey(s => s.ClientId);
            modelBuilder.Entity<BillDto>().ToTable("Bill").HasKey(s => s.BillId);
        }
    }
}
