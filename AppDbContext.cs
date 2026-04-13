using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<ShortLink> ShortLinks { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


}