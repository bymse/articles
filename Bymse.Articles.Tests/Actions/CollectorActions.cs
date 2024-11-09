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
            WebPage = new($"https://example.com/{random}")
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
        
        await client.ConfirmSourceAsync(new ConfirmSourceRequest
        {
            ReceivedEmailId = manualProcessingEmails.Items.Single().ReceivedEmailId
        });
        
        return (await client.GetSourcesAsync()).Items.Single();
    }
}