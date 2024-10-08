using Infrastructure.ServicesConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace Feeder.Infrastructure;

public static class FeederModule
{
    public static IServiceCollection AddFeederServices(this IServiceCollection services)
    {
        return services
            .AddApplicationHandlers(typeof(Application.FeederConstants).Assembly);
    }
}