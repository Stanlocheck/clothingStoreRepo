namespace ClothingStorePersistence;

public class ApplicationDbContext : DbContext
{
    DbSet<Cloth> Clothes { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
}
