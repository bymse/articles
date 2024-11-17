using Bymse.Articles.PublicApi.Client;
using Bymse.Articles.Tests.Emails;
using FluentAssertions;
using FluentAssertions.Extensions;
using Registry.Integration;

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
            WebPage = new("https://example.com"),
            Type = SourceType.BonoboEmailDigest
        };
        var unconfirmedSource = await client.CreateSourceAsync(request);

        var sources = await client.GetSourcesAsync();
        sources.Items
            .Should()
            .ContainEquivalentOf(new SourceInfo
            {
                Title = request.Title,
                Id = unconfirmedSource.Id.Value,
                WebPage = request.WebPage,
                State = SourceState.Unconfirmed,
                ReceiverEmail = unconfirmedSource.Email
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
            .ContainSingle(e => e.ToEmail == source.Email)
            .Which.Should()
            .BeEquivalentTo(new ManualProcessingEmailInfo
            {
                ReceivedEmailId = "",
                FromEmail = message.From,
                FromName = "",
                Subject = message.Subject,
                TextBody = message.Body,
                ToEmail = message.To,
                Type = ManualProcessingEmailType.ConfirmSubscription,
                HtmlBody = null
            }, e => e.Excluding(r => r.ReceivedEmailId));
    }

    [Test]
    public async Task Should_ConfirmSourceManually_OnReceivedConfirmationEmail()
    {
        var source = await Actions.Collector.CreateSource();
        await Actions.ExternalSystem.SendConfirmationEmail(source);
        var manualProcessingEmails = await Actions.Collector.GetManualProcessingEmails();
        var receivedEmail = manualProcessingEmails.Items.Single(e => e.ToEmail == source.Email);

        var client = GetPublicApiClient();
        await client.ConfirmSourceAsync(new ConfirmSourceRequest
        {
            ReceivedEmailId = receivedEmail.ReceivedEmailId
        });

        var sources = await client.GetSourcesAsync();

        sources.Items
            .Should()
            .ContainEquivalentOf(new
            {
                State = SourceState.Confirmed,
                Id = source.Id.Value,
                ReceiverEmail = source.Email
            });
    }

    [Test]
    public async Task Should_PublishSaveArticleTask_OnArticleEmailReceived()
    {
        var confirmedSource = await Actions.Collector.CreateConfirmedSource();
        await Actions.ExternalSystem.SendArticlesEmail(confirmedSource, "digest-email.html");

        var savedArticles = MessagesReceiver
            .GetReceivedMessages<SaveArticleTask>()
            .ToArray();

        savedArticles.Should().BeEquivalentTo([
            new { Url = new Uri("https://exmaple.com/3"), },
            new { Url = new Uri("https://exmaple.com/5"), },
            new { Url = new Uri("https://exmaple.com/6"), },
            new { Url = new Uri("https://exmaple.com/7"), },
            new { Url = new Uri("https://exmaple.com/8"), }
        ]);
    }
}