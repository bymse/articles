using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Collector.Infrastructure.Database;

public class CollectorDbContextFactory : IDesignTimeDbContextFactory<CollectorDbContext>
{
    public CollectorDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<CollectorDbContext>()
            .UseNpgsql()
            .Options;

        return new CollectorDbContext(options);
    }
}