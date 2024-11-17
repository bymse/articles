using Application.Contexts;
using Bymse.Articles.PublicApi.Client;
using Bymse.Articles.Tests.Actions;
using Bymse.Articles.Tests.Emails;
using Bymse.Articles.Tests.TestConsumers;
using Infrastructure.ServicesConfiguration;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Bymse.Articles.Tests.Infrastructure;

public class SelfHostedSettings
{
    public bool Enable { get; init; }

    public string SmtpHost { get; init; } = null!;
    public int SmtpPort { get; init; }

    public string RabbitMqConnectionString { get; init; } = null!;
    public string ApisUrl { get; init; } = null!;
}

public class SelfHostedArticlesTestHost : IArticlesTestHost
{
    private SelfHostedSettings settings = null!;

    private IHost host = null!;

    public PublicApiClient GetClient()
    {
        return new PublicApiClient(settings.ApisUrl, new HttpClient());
    }

    public IEnumerable<T> GetReceivedMessages<T>() =>
        host
            .Services
            .GetRequiredService<MessagesReceiver>()
            .GetReceivedMessages<T>();

    public IArticlesActions Actions => host.Services.GetRequiredService<IArticlesActions>();

    public bool CanStart()
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddEnvironmentVariables("ARTICLES_")
            .Build();

        settings = configurationBuilder.Get<SelfHostedSettings>() ?? new SelfHostedSettings();
        return settings.Enable;
    }

    public async Task Start()
    {
        if (!settings.Enable)
        {
            throw new InvalidOperationException("Self-hosted is not enabled");
        }

        host = Host.CreateDefaultBuilder()
            .ConfigureServices(r => AddMassTransit(r)
                .AddArticlesTestServices(_ => new SmtpEmailSender(settings.SmtpHost, settings.SmtpPort))
                .AddSingleton<PublicApiClient>(_ => GetClient())
            )
            .Build();

        await host.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await host.StopAsync();
        host.Dispose();
    }

    private IServiceCollection AddMassTransit(IServiceCollection services)
    {
        services
            .AddSingleton<MessagesReceiver>()
            .AddScoped<ConsumeContextManager>()
            .AddMassTransit(e =>
            {
                e.AddConsumer<GenericConsumer>();
                e.UsingArticlesRabbitMq(() => settings.RabbitMqConnectionString);
            })
            .AddOptions<MassTransitHostOptions>()
            .Configure(options => { options.WaitUntilStarted = true; });

        return services;
    }
}