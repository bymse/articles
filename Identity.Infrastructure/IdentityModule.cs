using Identity.Infrastructure.Database;
using Infrastructure.ServicesConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        return services
            .AddPostgresDbContext<IdentityDbContext>()
            .AddUseCases(typeof(Application.Identity).Assembly);
    }
}