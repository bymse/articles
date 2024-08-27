using Identity.Integration;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

public class IdentityDbContext(DbContextOptions options) : DbContext(options)
{
    public const string ConnectionName = "identity";

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