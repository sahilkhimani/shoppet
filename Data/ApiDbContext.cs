using Microsoft.EntityFrameworkCore;
using PetShopApi.Models;

namespace PetShopApi.Data
{
    public class ApiDbContext : DbContext
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
        }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
