using Collector.Infrastructure.Database;
using Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Collector.Infrastructure;

public static class CollectorModule
{
    public static IServiceCollection AddCollectorServices(this IServiceCollection services)
    {
        return services
            .AddPostgresDbContext<CollectorDbContext>(nameof(Application.Collector))
            .AddUseCases(typeof(Application.Collector).Assembly);
    }
}