using Bymse.Articles.PublicApi.Client;

namespace Bymse.Articles.Tests.Actions;

public class CollectorActions(PublicApiClient client, IExternalSystemActions externalSystem) : ICollectorActions
{
    public Task<UnconfirmedSourceInfo> CreateSource()
    {
        var random = Guid.NewGuid().ToString();
        var request = new CreateSourceRequest
        {
            Title = random,
            WebPage = new($"https://example.com/{random}"),
            Type = SourceType.BonoboEmailDigest
        };
        return client.CreateSourceAsync(request);
    }

    public Task<ManualProcessingEmailInfoCollection> GetManualProcessingEmails()
    {
        return client.GetManualProcessingEmailsAsync();
    }

    public async Task<SourceInfo> CreateConfirmedSource()
    {
        var source = await CreateSource();
        await externalSystem.SendConfirmationEmail(source);
        var manualProcessingEmails = await GetManualProcessingEmails();
        
        var receivedEmail = manualProcessingEmails
            .Items
            .Single(e => e.ToEmail == source.Email);
        
        await client.ConfirmSourceAsync(new ConfirmSourceRequest
        {
            ReceivedEmailId = receivedEmail.ReceivedEmailId
        });
        
        return (await client.GetSourcesAsync()).Items.Single(s => s.Id == source.Id.Value);
    }
}