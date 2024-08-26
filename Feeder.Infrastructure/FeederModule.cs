using Feeder.Infrastructure.Database;
using Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Feeder.Infrastructure;

public static class FeederModule
{
    public static IServiceCollection AddFeederServices(this IServiceCollection services)
    {
        return services
            .AddPostgresDbContext<FeederDbContext>(nameof(Application.Feeder))
            .AddUseCases(typeof(Application.Feeder).Assembly);
    }
}