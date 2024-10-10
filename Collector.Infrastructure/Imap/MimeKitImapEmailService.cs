using System.Runtime.CompilerServices;
using Collector.Application.Services;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Collector.Infrastructure.Imap;

public class MimeKitImapEmailService(IOptions<ImapEmailServiceSettings> settings) : IImapEmailService
{
    private readonly ImapEmailServiceSettings settings = settings.Value;

    public async IAsyncEnumerable<EmailModel> GetMessages(uint? uidValidity,
        uint? lastUid,
        [EnumeratorCancellation] CancellationToken ct)
    {
        using var client = new ImapClient();

        await client.ConnectAsync(settings.Hostname, settings.Port, useSsl: true, ct);
        await client.AuthenticateAsync(settings.Username, settings.Password, ct);

        var inbox = client.Inbox;
        await inbox.OpenAsync(FolderAccess.ReadOnly, ct);

        if (uidValidity.HasValue && inbox.UidValidity != uidValidity)
        {
            lastUid = null;
        }

        while (!ct.IsCancellationRequested)
        {
            var start = lastUid.HasValue ? new UniqueId(lastUid.Value + 1) : UniqueId.MinValue;
            var query = SearchQuery.Uids(new UniqueIdRange(start, UniqueId.MaxValue));
            var foundIds = await inbox.SearchAsync(query, ct);

            foreach (var uid in foundIds)
            {
                using var message = await inbox.GetMessageAsync(uid, ct);
                var email = new EmailModel
                {
                    ToEmail = message.To.Mailboxes.First().Address,
                    Subject = message.Subject,
                    FromEmail = message.From.Mailboxes.First().Address,
                    Date = message.Date,
                    TextBody = message.TextBody,
                    HtmlBody = message.HtmlBody,
                    Headers = message.Headers.ToDictionary(x => x.Field, x => x.Value),

                    UidValidity = inbox.UidValidity,
                    Uid = uid.Id
                };

                yield return email;
                lastUid = uid.Id;
            }

            await Task.Delay(TimeSpan.FromMinutes(1), ct);
        }

        await client.DisconnectAsync(true, ct);
    }
}