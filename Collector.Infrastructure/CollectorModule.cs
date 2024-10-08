using Collector.Application.Services;
using Collector.Application.Settings;
using Collector.Infrastructure.Database;
using Collector.Infrastructure.Imap;
using Infrastructure.ServicesConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace Collector.Infrastructure;

public static class CollectorModule
{
    public static IServiceCollection AddCollectorServices(this IServiceCollection services)
    {
        var assembly = typeof(Application.CollectorConstants).Assembly;

        services
            .AddOptions<CollectorApplicationSettings>()
            .BindConfiguration(CollectorApplicationSettings.Path);

        services
            .AddOptions<ImapEmailServiceSettings>()
            .BindConfiguration(ImapEmailServiceSettings.Path);

        return services
                .AddPostgresDbContext<CollectorDbContext>()
                .AddUseCases(assembly)
                .AddConsumers(assembly)
                .AddTransient<IImapEmailService, MimeKitImapEmailService>()
            ;
    }
}