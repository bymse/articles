using Identity.Integration;
using Infrastructure.ServicesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options), IKeyedDbContext
{
    public static string Key => nameof(Application.Identity);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<IdentityUserId>()
            .HaveConversion<IdentityUserIdConverter>();
    }
}