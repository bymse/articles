using Bymse.Articles.PublicApi.Client;
using Bymse.Articles.Tests.Actions;
using Bymse.Articles.Tests.Infrastructure;

namespace Bymse.Articles.Tests;

public abstract class TestsBase
{
    private static IEnumerable<IArticlesTestHost> TestHosts
    {
        get
        {
            yield return new SelfHostedArticlesTestHost();
            yield return new AspireTestingArticlesTestHost();
        }
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