using Collector.Application.Entities;
using Collector.Integration;
using Infrastructure.Ulids;
using Microsoft.EntityFrameworkCore;

namespace Collector.Infrastructure.Database;

public class CollectorDbContext(DbContextOptions options) : DbContext(options)
{
    public const string ConnectionName = "collector";
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<CollectorSourceId>()
            .HaveConversion<CollectorSourceIdConverter>();

        configurationBuilder.ComplexProperties<Receiver>();
    }
}