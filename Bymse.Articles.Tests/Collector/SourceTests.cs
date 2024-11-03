using Bymse.Articles.PublicApi.Client;
using Bymse.Articles.Tests.Emails;
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

    [Test]
    public async Task Should_ReceiveEmailForManualProcessing_OnUnconfirmedSource()
    {
        var source = await Actions.Collector.CreateSource();

        await EmailSender.SendEmail(new EmailMessage(source.Email, "Test subject", "Test body", BodyType.PlainText,
            "me@localhost"));

        await Task.Delay(TimeSpan.FromSeconds(15));
        var client = GetPublicApiClient();
        var manualProcessingEmails = await client.GetManualProcessingEmailsAsync();

        manualProcessingEmails.Items
            .Should()
            .ContainSingle()
            .Which
            .ToEmail.Should().Be(source.Email);
    }
}