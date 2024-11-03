using Bymse.Articles.PublicApi.Client;
using FluentAssertions;

namespace Bymse.Articles.Tests.Collector;

public class SourceTests : TestsBase
{
    [Test]
    public async Task Should_CreateUnconfirmedSource_OnPublicApi()
    {
        var client = GetPublicApiClient();

        var request = new CreateSourceRequest
        {
            Title = "First source",
            WebPage = new("https://example.com")
        };
        var unconfirmedSource = await client.CreateSourceAsync(request);

        var sources = await client.GetSourcesAsync();
        sources.Should().BeEquivalentTo(new SourceInfoCollection
        {
            Items =
            [
                new SourceInfo
                {
                    Title = request.Title,
                    Id = unconfirmedSource.Id.Value,
                    WebPage = request.WebPage,
                    State = SourceState.Unconfirmed,
                    ReceiverEmail = unconfirmedSource.Email
                }
            ]
        });
    }
}