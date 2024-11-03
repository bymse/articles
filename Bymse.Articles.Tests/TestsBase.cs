using Bymse.Articles.AppHost;
using Bymse.Articles.PublicApi.Client;
using Bymse.Articles.Tests.Actions;
using Bymse.Articles.Tests.Emails;

namespace Bymse.Articles.Tests;

public abstract class TestsBase
{
    private DistributedApplication app;

    protected PublicApiClient GetPublicApiClient()
    {
        var httpClient = app.CreateHttpClient(ArticlesResources.Apis);
        return new PublicApiClient(httpClient);
    }

    protected IArticlesActions Actions => app.Services.GetRequiredService<IArticlesActions>();
    
    protected IEmailSender EmailSender => app.Services.GetRequiredService<IEmailSender>();

    [SetUp]
    public async Task SetUp()
    {
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Bymse_Articles_AppHost>(["--no-volumes", "--run-green-mail"]);

        appHost.Services
            .AddSingleton<IArticlesActions, ArticlesActions>()
            .AddSingleton<ICollectorActions, CollectorActions>()
            .AddSingleton(_ => GetPublicApiClient())
            .AddSingleton<IEmailSender, SmtpEmailSender>()
            ;

        app = await appHost.BuildAsync();
        var resourceNotificationService = app.Services.GetRequiredService<ResourceNotificationService>();
        await app.StartAsync();

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

    [TearDown]
    public async Task TearDown()
    {
        await app.StopAsync();
        await app.DisposeAsync();
    }
}