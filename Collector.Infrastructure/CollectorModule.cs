using Collector.Application.Settings;
using Collector.Infrastructure.Imap;
using Infrastructure.Di;
using Infrastructure.ServicesConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace Collector.Infrastructure;

public static class CollectorModule
{
    public static IServiceCollection AddCollectorServices(this IServiceCollection services)
    {
        var applicationAssembly = typeof(Application.CollectorConstants).Assembly;
        var infrastructureAssembly = typeof(CollectorModule).Assembly;

        services
            .AddOptions<CollectorApplicationSettings>()
            .BindConfiguration(CollectorApplicationSettings.Path);

        services
            .AddOptions<ImapEmailServiceSettings>()
            .BindConfiguration(ImapEmailServiceSettings.Path);

        return services
                .AddApplicationHandlers(applicationAssembly)
                .AddConsumers(infrastructureAssembly)
                .AddAutoRegistrations(applicationAssembly, infrastructureAssembly)
                .AddScoped<ImapClientFactory>()
            ;
    }
}