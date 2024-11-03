using Bymse.Articles.PublicApi.Client;

namespace Bymse.Articles.Tests.Actions;

public class CollectorActions(PublicApiClient client) : ICollectorActions
{
    public Task<UnconfirmedSourceInfo> CreateSource()
    {
        var random = Guid.NewGuid().ToString();
        var request = new CreateSourceRequest
        {
            Title = random,
            WebPage = new($"https://example.com/{random}")
        };
        return client.CreateSourceAsync(request);
    }
}