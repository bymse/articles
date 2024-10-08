using Collector.Application.Entities;
using Collector.Infrastructure;
using Collector.Infrastructure.Database;
using Collector.Integration;
using Feeder.Application.Entities;
using Feeder.Infrastructure;
using Feeder.Infrastructure.Database;
using Identity.Infrastructure;
using Identity.Infrastructure.Database;
using Identity.Integration;
using Infrastructure.Ulids;
using Microsoft.EntityFrameworkCore;

namespace Bymse.Articles.Database;

public class ArticlesDbContext(DbContextOptions<ArticlesDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CollectorModule).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityModule).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FeederModule).Assembly);
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

        configurationBuilder.ComplexProperties<Receiver>();

        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidValueConverter>();
    }
}