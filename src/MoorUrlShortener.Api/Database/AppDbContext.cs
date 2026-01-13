using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MoorUrlShortener.Api.Domain.Entities.ShortenedUrl;

namespace MoorUrlShortener.Api.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
