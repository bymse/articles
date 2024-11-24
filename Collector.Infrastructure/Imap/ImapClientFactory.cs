using MailKit.Net.Imap;
using Microsoft.Extensions.Options;

namespace Collector.Infrastructure.Imap;

public class ImapClientFactory(IOptions<ImapEmailServiceSettings> options) : IAsyncDisposable
{
    private readonly ImapClient client = new();

    public async Task<IImapClient> Create()
    {
        if (client.IsConnected)
        {
            return client;
        }

        var settings = options.Value;

        await client.ConnectAsync(settings.Hostname, settings.Port, settings.UseSsl);
        await client.AuthenticateAsync(settings.Username, settings.Password);

        return client;
    }

    public async ValueTask DisposeAsync()
    {
        await client.DisconnectAsync(true);
        client.Dispose();
    }
}