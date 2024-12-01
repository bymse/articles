using System.Runtime.CompilerServices;
using Application.Di;
using Collector.Application.Services;
using MailKit;
using MailKit.Search;

namespace Collector.Infrastructure.Imap;

[AutoRegistration]
public class MailKitImapEmailService(ImapClientFactory clientFactory, CollectorMetrics metrics) : IImapEmailService
{
    public async IAsyncEnumerable<EmailModel> GetMessages(uint? uidValidity,
        uint? lastUid,
        int count,
        [EnumeratorCancellation] CancellationToken ct)
    {
        var client = await clientFactory.Create();

        var inbox = client.Inbox;

        if (!inbox.IsOpen)
        {
            await inbox.OpenAsync(FolderAccess.ReadOnly, ct);
        }

        if (uidValidity.HasValue && inbox.UidValidity != uidValidity)
        {
            lastUid = null;
        }

        var start = lastUid.HasValue ? new UniqueId(lastUid.Value + 1) : UniqueId.MinValue;
        var end = new UniqueId((uint)(start.Id + count));
        var query = SearchQuery.Uids(new UniqueIdRange(start, end));
        var foundIds = await inbox.SearchAsync(query, ct);

        foreach (var uid in foundIds)
        {
            ct.ThrowIfCancellationRequested();
            using var message = await inbox.GetMessageAsync(uid, ct);
            var from = message.From.Mailboxes.First();
            var email = new EmailModel
            {
                ToEmail = message.To.Mailboxes.First().Address,
                Subject = message.Subject,
                FromEmail = from.Address,
                FromName = from.Name,
                Date = message.Date,
                TextBody = message.TextBody,
                HtmlBody = message.HtmlBody,
                Headers = message
                    .Headers
                    .ToLookup(x => x.Field, x => x.Value)
                    .ToDictionary(e => e.Key, e => string.Join(",", e)),

                UidValidity = inbox.UidValidity,
                Uid = uid.Id
            };
            
            metrics.ReportFetched();
            yield return email;
        }
    }
}