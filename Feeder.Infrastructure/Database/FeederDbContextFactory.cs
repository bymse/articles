using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Feeder.Infrastructure.Database;

public class FeederDbContextFactory : IDesignTimeDbContextFactory<FeederDbContext>
{
    public FeederDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<FeederDbContext>()
            .UseNpgsql()
            .UseSnakeCaseNamingConvention()
            .Options;

        return new FeederDbContext(options);
    }
}