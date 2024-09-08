using Collector.Infrastructure.Database;
using Collector.Integration;
using Feeder.Application.Entities;
using Identity.Infrastructure.Database;
using Identity.Integration;
using Infrastructure.ServicesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Feeder.Infrastructure.Database;

public class FeederDbContext(DbContextOptions<FeederDbContext> options) : DbContext(options), IKeyedDbContext
{
    public static string Key => nameof(Application.Feeder);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<CollectorSourceId>()
            .HaveConversion<CollectorSourceIdConverter>();

        configurationBuilder
            .Properties<IdentityUserId>()
            .HaveConversion<IdentityUserIdConverter>();

        configurationBuilder
            .Properties<FeedId>()
            .HaveConversion<FeedIdConverter>();
    }
}