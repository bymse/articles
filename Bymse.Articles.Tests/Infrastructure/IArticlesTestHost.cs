using Bymse.Articles.PublicApi.Client;
using Bymse.Articles.Tests.Actions;
using Bymse.Articles.Tests.TestConsumers;

namespace Bymse.Articles.Tests.Infrastructure;

public interface IArticlesTestHost : IAsyncDisposable
{
    bool CanStart();
    Task Start();
    PublicApiClient GetClient();
    IEnumerable<T> GetReceivedMessages<T>();

    IArticlesActions Actions { get; }
}