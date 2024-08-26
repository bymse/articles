using Identity.Infrastructure.Database;
using Infrastructure.DI;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class IdentityModule
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        return services
            .AddPostgresDbContext<IdentityDbContext>(nameof(Application.Identity))
            .AddUseCases(typeof(Application.Identity).Assembly);
    }
}