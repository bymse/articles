using Application.DbContexts;
using Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Infrastructure.DI;

public static class DbContextDiHelper
{
    public static IServiceCollection AddPostgresDbContext<TDbContext>(this IServiceCollection services, string key)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>((e, r) =>
        {
            var settings = e.GetRequiredService<IOptions<PostgresSettings>>();
            r.UseNpgsql(settings.Value.ConnectionString);
        });

        services.AddKeyedScoped<DbContext>(key, (e, _) => e.GetRequiredService<TDbContext>());

        services.TryAddScoped(typeof(IDbContextProvider<>), typeof(DbContextProvider<>));

        return services;
    }
}