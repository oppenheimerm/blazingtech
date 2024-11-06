using BT.Shared.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BT.Authentication.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<BTUser> ApplicationUser { get; set; } = default!;
        public DbSet<Role> Role { get; set; } = default!;
        public DbSet<UserRole> UserRole { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //  Many-to-many with class for join entity
            //  The UsingEntity method can be used to configure this as the join entity(UserRole)  type for the relationship. For example:
            modelBuilder.Entity<BTUser>()
                .HasMany(r => r.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRole>();
        }
    }
}

