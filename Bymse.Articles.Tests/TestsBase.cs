using Application.Contexts;
using Bymse.Articles.AppHost;
using Bymse.Articles.PublicApi.Client;
using Bymse.Articles.Tests.Actions;
using Bymse.Articles.Tests.Emails;
using Bymse.Articles.Tests.Infrastructure;
using Bymse.Articles.Tests.TestConsumers;
using Infrastructure.ServicesConfiguration;
using MassTransit;
using MassTransit.Monitoring;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Bymse.Articles.Tests;

public abstract class TestsBase
{
    private static IEnumerable<IArticlesTestHost> TestHosts
    {
        get { yield return new AspireTestingArticlesTestHost(); }
    }

    private IArticlesTestHost testHost = null!;

    protected PublicApiClient GetPublicApiClient() => testHost.GetClient();

    protected IArticlesActions Actions => testHost.Actions;

    protected IEnumerable<T> GetReceivedMessages<T>() => testHost.GetReceivedMessages<T>();

    [OneTimeSetUp]
    public async Task SetUp()
    {
        testHost = TestHosts.First(e => e.CanStart());
        await testHost.Start();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await testHost.DisposeAsync();
    }
}