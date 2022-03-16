using Microsoft.EntityFrameworkCore;
using ShopbridgeWebAPI.Domain.Models;

namespace ShopbridgeWebAPI.Data
{
    public class Shopbridge_Context : DbContext
    {
        public Shopbridge_Context (DbContextOptions<Shopbridge_Context> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
                .HasIndex(product=>product.Name)
                .IsUnique()
                .HasDatabaseName("UQ_Product_Name");

        }
    }
}
