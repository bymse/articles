using Bymse.Articles.Database;
using Microsoft.EntityFrameworkCore;

namespace DbMigrator;

public class DbMigratorWorker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var sp = scope.ServiceProvider;
        await MigrateAsync(sp.GetRequiredService<ArticlesDbContext>(), cancellationToken);

        hostApplicationLifetime.StopApplication();
    }
    
    private static async Task MigrateAsync(DbContext context, CancellationToken ct)
    {
        await context.Database.MigrateAsync(ct);
    }
}