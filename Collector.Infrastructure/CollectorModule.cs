using Collector.Application.Services;
using Collector.Application.Settings;
using Collector.Infrastructure.Database;
using Collector.Infrastructure.Html;
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
                .AddApplicationHandlers(assembly)
                .AddConsumers(assembly)
                .AddScoped<IImapEmailService, MimeKitImapEmailService>()
                .AddScoped<EmailClassifier>()
                .AddScoped<IHtmlLinksFinder, AngleSharpHtmlLinksFinder>()
            ;
    }
}