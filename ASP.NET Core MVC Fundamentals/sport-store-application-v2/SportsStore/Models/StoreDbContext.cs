using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models;

// NEW: Add StoreDbContext class
public class StoreDbContext(DbContextOptions<StoreDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => this.Set<Product>();
    public DbSet<Order> Orders => this.Set<Order>();
}
