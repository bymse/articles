using Application.DbContexts;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Infrastructure.ServicesConfiguration;

public static class DbContextServicesConfiguration
{
    public static IServiceCollection AddPostgresDbContext<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext, IKeyedDbContext
    {
        services.AddDbContext<TDbContext>((e, r) =>
        {
            var configuration = e.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(TDbContext.Key) ??
                                   throw new Exception("Connection string not found for " + TDbContext.Key);
            r.UseNpgsql(connectionString);
        });

        services.AddKeyedScoped<DbContext>(TDbContext.Key, (e, _) => e.GetRequiredService<TDbContext>());

        services.TryAddScoped(typeof(IUseCaseDbContextProvider), typeof(UseCaseDbContextProvider));

        return services;
    }
}