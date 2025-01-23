using Microsoft.EntityFrameworkCore;
using ClothDomain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ClothingStorePersistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Cloth> Clothes { get; set; } = null!;
    public DbSet<Buyer> Buyers { get; set; } = null!;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder){}
}
