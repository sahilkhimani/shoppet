using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetShopApi.Models;

namespace PetShopApi.Data
{
    public class ApiDbContext : IdentityDbContext<User, Role, string>
    {
        public ApiDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Order>()
                       .HasOne(o => o.Buyer)
                       .WithMany(u => u.Orders)
                       .HasForeignKey(o => o.BuyerId)
                       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Pet)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>().HasData(
        new Role { Id = "1", Name = "Admin", NormalizedName = "ADMIN" , RoleDescription ="Admin Role" },
        new Role { Id = "2", Name = "Seller", NormalizedName = "SELLER" , RoleDescription = "Seller Role"},
        new Role { Id = "3", Name = "Buyer", NormalizedName = "BUYER" , RoleDescription = "Buyer Role"}
    );
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(x => new { x.LoginProvider, x.ProviderKey });
            modelBuilder.Entity<IdentityUserRole<string>>().HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserToken<string>>().HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

        }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
