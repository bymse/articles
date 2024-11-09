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

        var message = await Actions.ExternalSystem.SendConfirmationEmail(source);

        var client = GetPublicApiClient();
        var manualProcessingEmails = await client.GetManualProcessingEmailsAsync();

        manualProcessingEmails.Items
            .Should()
            .ContainSingle()
            .Which
            .Should().BeEquivalentTo(new ManualProcessingEmailInfo
                {
                    ReceivedEmailId = "",
                    FromEmail = message.From,
                    FromName = "",
                    Subject = message.Subject,
                    TextBody = message.Body,
                    ToEmail = message.To,
                    Type = ManualProcessingEmailType.ConfirmSubscription,
                    HtmlBody = null
                }, e => e.Excluding(r => r.ReceivedEmailId)
            );
    }

    [Test]
    public async Task Should_ConfirmSourceManually_OnReceivedConfirmationEmail()
    {
        var source = await Actions.Collector.CreateSource();
        await Actions.ExternalSystem.SendConfirmationEmail(source);
        var manualProcessingEmails = await Actions.Collector.GetManualProcessingEmails();

        var client = GetPublicApiClient();
        await client.ConfirmSourceAsync(new ConfirmSourceRequest
        {
            ReceivedEmailId = manualProcessingEmails.Items.Single().ReceivedEmailId
        });

        var sources = await client.GetSourcesAsync();

        sources.Items
            .Should()
            .ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                State = SourceState.Confirmed,
                Id = source.Id.Value,
                ReceiverEmail = source.Email
            });
    }
}