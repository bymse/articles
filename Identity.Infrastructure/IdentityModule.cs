using Identity.Infrastructure.Database;
using Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class IdentityModule
{
    private const string Key = nameof(Application.Identity);
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddPostgresDbContext<IdentityDbContext>(Key);
    }
}