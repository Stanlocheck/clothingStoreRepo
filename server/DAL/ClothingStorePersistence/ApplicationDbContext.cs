using Microsoft.EntityFrameworkCore;
using ClothDomain;

namespace ClothingStorePersistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Cloth> Clothes { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
}
