using Collector.Application.Services;
using Collector.Application.Settings;
using Collector.Infrastructure.Database;
using Collector.Infrastructure.Imap;
using Infrastructure.ServicesConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace Collector.Infrastructure;

public static class CollectorModule
{
    public static IServiceCollection AddCollectorServices(this IServiceCollection services, bool includeConsumers = false)
    {
        services
            .AddOptions<CollectorApplicationSettings>()
            .BindConfiguration(CollectorApplicationSettings.Path);

        services
            .AddOptions<ImapEmailServiceSettings>()
            .BindConfiguration(ImapEmailServiceSettings.Path);

        if (includeConsumers)
        {
            services.AddConsumersFrom(typeof(CollectorModule).Assembly);
        }

        return services
            .AddPostgresDbContext<CollectorDbContext>()
            .AddUseCases(typeof(Application.CollectorConstants).Assembly)
            .AddTransient<IImapEmailService, MimeKitImapEmailService>()
            ;
    }
}