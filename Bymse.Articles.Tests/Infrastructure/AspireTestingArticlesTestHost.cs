using Application.Contexts;
using Bymse.Articles.AppHost;
using Bymse.Articles.PublicApi.Client;
using Bymse.Articles.Tests.Actions;
using Bymse.Articles.Tests.Emails;
using Bymse.Articles.Tests.TestConsumers;
using Infrastructure.ServicesConfiguration;
using MassTransit;
using MassTransit.Monitoring;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Options;

namespace Bymse.Articles.Tests.Infrastructure;

public class AspireTestingArticlesTestHost : IArticlesTestHost
{
    private DistributedApplication app = null!;
    private MassTransitHostedService massTransitHostedService = null!;
    private string? rabbitMqConnectionString;

    public IArticlesActions Actions => app.Services.GetRequiredService<IArticlesActions>();

    public PublicApiClient GetClient()
    {
        var httpClient = app.CreateHttpClient(ArticlesResources.Apis);
        return new PublicApiClient(httpClient);
    }

    public IEnumerable<T> GetReceivedMessages<T>() =>
        app.Services
            .GetRequiredService<MessagesReceiver>()
            .GetReceivedMessages<T>();

    public bool CanStart() => true;

    public async Task Start()
    {
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Bymse_Articles_AppHost>(["--no-volumes", "--run-green-mail"]);

        AddMassTransit(appHost.Services)
            .AddArticlesTestServices(_ => new SmtpEmailSender())
            .AddSingleton(GetClient);

        app = await appHost.BuildAsync();
        await app.StartAsync();

        await WaitForServices();

        rabbitMqConnectionString = await app.GetConnectionStringAsync(ArticlesResources.RabbitMq);
        massTransitHostedService = new MassTransitHostedService(
            app.Services.GetRequiredService<IBusDepot>(),
            app.Services.GetRequiredService<IOptions<MassTransitHostOptions>>()
        );
        await massTransitHostedService.StartAsync(ApplicationLifetime.ApplicationStopping);
    }

    public async ValueTask DisposeAsync()
    {
        await massTransitHostedService.StopAsync(ApplicationLifetime.ApplicationStopped);
        await massTransitHostedService.DisposeAsync();
        await app.StopAsync();
        await app.DisposeAsync();
    }

    private IHostApplicationLifetime ApplicationLifetime => app.Services.GetRequiredService<IHostApplicationLifetime>();

    private async Task WaitForServices()
    {
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await resourceNotificationService
            .WaitForResourceAsync(ArticlesResources.DbMigrator, KnownResourceStates.Finished)
            .WaitAsync(TimeSpan.FromSeconds(60));

        var resourcesToWait = ArticlesResources.Services
            .Select(service => resourceNotificationService
                .WaitForResourceAsync(service, KnownResourceStates.Running)
                .WaitAsync(TimeSpan.FromSeconds(60)))
            .ToArray();

        await Task.WhenAll(resourcesToWait);
    }

    private IServiceCollection AddMassTransit(IServiceCollection services)
    {
        services
            .AddSingleton<MessagesReceiver>()
            .AddScoped<ConsumeContextManager>()
            .AddMassTransit(e =>
            {
                e.AddConsumer<GenericConsumer>();
                e.UsingArticlesRabbitMq(() => rabbitMqConnectionString);
            })
            .AddOptions<MassTransitHostOptions>()
            .Configure(options => { options.WaitUntilStarted = true; });

        services.RemoveMassTransitHostedService();

        var toRemove = services.Single(e => e.ImplementationType == typeof(ConfigureBusHealthCheckServiceOptions));
        services.Remove(toRemove);

        return services;
    }
}