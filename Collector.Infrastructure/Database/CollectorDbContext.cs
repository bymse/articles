using Infrastructure.Ulids;
using Microsoft.EntityFrameworkCore;

namespace Collector.Infrastructure.Database;

public class CollectorDbContext : DbContext
{
    public CollectorDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidToStringConverter>();

        configurationBuilder
            .Properties<CollectorSourceIdConverter>()
            .HaveConversion<CollectorSourceIdConverter>();
    }
}