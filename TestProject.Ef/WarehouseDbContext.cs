using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Core;

namespace TestProject.Ef
{
    public class WarehouseDbContext : DbContext
    { 
        public DbSet<Product> Products { get; set; } 

        public WarehouseDbContext()
        {
            
        }

        public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             

            modelBuilder.Entity<Product>(e =>
            {
                e.HasData(new[]
                {
                    new Product()
                    {
                        Id = 41,
                        Price = 100,
                        Quantity = 112
                    },
                    new Product()
                    {
                        Id = 42,
                        Price = 200,
                        Quantity = 34
                    },
                    new Product()
                    {
                        Id = 43,
                        Price = 300,
                        Quantity = 323
                    },
                });
            });
        }

    }

    
}
