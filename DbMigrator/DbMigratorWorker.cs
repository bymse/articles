using Collector.Infrastructure.Database;
using Feeder.Infrastructure.Database;
using Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DbMigrator;

public class DbMigratorWorker(
    IServiceProvider serviceProvider)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;
        await MigrateAsync(sp.GetRequiredService<IdentityDbContext>(), cancellationToken);
        await MigrateAsync(sp.GetRequiredService<CollectorDbContext>(), cancellationToken);
        await MigrateAsync(sp.GetRequiredService<FeederDbContext>(), cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private static async Task MigrateAsync(DbContext context, CancellationToken ct)
    {
        await context.Database.MigrateAsync(ct);
    }
}