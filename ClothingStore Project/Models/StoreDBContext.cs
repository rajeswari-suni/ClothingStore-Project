using Microsoft.EntityFrameworkCore;

namespace ClothingStore_Project.Models
{
    public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
    {
        public  DbSet<Product> Products { get; set; }
        public  DbSet<Order> Orders { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public  DbSet<Seller> Sellers  { get; set; }
        public  DbSet<Agent> Agents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>();
        }
    }
}
