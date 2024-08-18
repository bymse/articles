using Collector.Application.Entities;
using Collector.Integration;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<CollectorSourceId>()
            .HaveConversion<CollectorSourceIdConverter>();

        configurationBuilder.ComplexProperties<Receiver>();
    }
}