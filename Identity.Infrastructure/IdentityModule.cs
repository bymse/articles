using Identity.Application;
using Identity.Infrastructure.Database;
using Infrastructure.ServicesConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services
            .AddOptions<IdentityApplicationSettings>()
            .BindConfiguration(IdentityApplicationSettings.Path);
        
        return services
            .AddApplicationHandlers(typeof(IdentityConstants).Assembly);
    }
}