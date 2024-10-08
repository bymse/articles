using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ServicesConfiguration;

public static class DbContextServicesConfiguration
{
    public static IServiceCollection AddPostgresDbContext<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>((e, r) =>
        {
            var configuration = e.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("pg-articles") ??
                                   throw new Exception("Connection string not found for pg");

            r.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<DbContext>(e => e.GetRequiredService<DbContext>());

        return services;
    }
}