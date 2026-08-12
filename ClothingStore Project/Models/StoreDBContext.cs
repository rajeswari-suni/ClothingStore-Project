using Microsoft.EntityFrameworkCore;

namespace ClothingStore_Project.Models
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Seller> Sellers { get; set; }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<AddressDetails> Addresses { get; set; }
    }
}
