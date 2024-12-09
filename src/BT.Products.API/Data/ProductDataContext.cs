using BT.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace BT.Products.API.Data
{
    public class ProductDataContext : DbContext
    {
        public ProductDataContext(DbContextOptions<ProductDataContext> options)
        : base(options)
        {
        }

        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<ProductImage> ProductImage { get; set; } = default!;
        public DbSet<ProductSpecfication> ProductSpecfication { get; set; } = default!;
    }
}
