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
using Microsoft.Extensions.Options;

namespace Bymse.Articles.Tests;

public abstract class TestsBase
{
    private DistributedApplication app;
    private MassTransitHostedService massTransitHostedService;
    private string? rabbitMqConnectionString;

    private IHostApplicationLifetime ApplicationLifetime => app.Services.GetRequiredService<IHostApplicationLifetime>();

    protected PublicApiClient GetPublicApiClient()
    {
        var httpClient = app.CreateHttpClient(ArticlesResources.Apis);
        return new PublicApiClient(httpClient);
    }

    protected IArticlesActions Actions => app.Services.GetRequiredService<IArticlesActions>();

    protected MessagesReceiver MessagesReceiver => app.Services.GetRequiredService<MessagesReceiver>();

    [SetUp]
    public async Task SetUp()
    {
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Bymse_Articles_AppHost>(["--no-volumes", "--run-green-mail"]);

        AddTestServices(appHost.Services);

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

    private void AddTestServices(IServiceCollection services)
    {
        services
            .AddSingleton<IArticlesActions, ArticlesActions>()
            .AddSingleton<ICollectorActions, CollectorActions>()
            .AddSingleton<IExternalSystemActions, ExternalSystemActions>()
            .AddSingleton<IEmailSender, SmtpEmailSender>()
            .AddSingleton(new MessagesReceiver())
            .AddScoped<ConsumeContextManager>()
            .AddSingleton(_ => GetPublicApiClient())
            .AddMassTransit(e =>
            {
                e.UsingArticlesRabbitMq(() => rabbitMqConnectionString);
                e.AddConsumer<GenericConsumer>();
            });

        services.RemoveMassTransitHostedService();

        var toRemove = services.Single(e => e.ImplementationType == typeof(ConfigureBusHealthCheckServiceOptions));
        services.Remove(toRemove);
    }

    [TearDown]
    public async Task TearDown()
    {
        await massTransitHostedService.StopAsync(ApplicationLifetime.ApplicationStopped);
        await massTransitHostedService.DisposeAsync();
        await app.StopAsync();
        await app.DisposeAsync();
    }
}