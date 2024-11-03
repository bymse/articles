using Collector.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.GetManualProcessingEmails;

public class GetManualProcessingEmailsHandler(DbContext context)
{
    public async Task<ManualProcessingEmailInfoCollection> Handle(CancellationToken ct)
    {
        var items = await context.Set<ManualProcessingEmail>()
            .Where(e => !e.IsProcessed)
            .Join(context.Set<ReceivedEmail>(),
                e => e.ReceivedEmailId,
                e => e.Id,
                (m, r) => new { ManualProcessingEmail = m, ReceivedEmail = r })
            .Select(e => new ManualProcessingEmailInfo
            {
                ReceivedEmailId = e.ReceivedEmail.Id,
                Type = e.ManualProcessingEmail.Type,
                ToEmail = e.ReceivedEmail.ToEmail,
                FromEmail = e.ReceivedEmail.FromEmail,
                FromName = e.ReceivedEmail.FromName,
                Subject = e.ReceivedEmail.Subject,
                TextBody = e.ReceivedEmail.TextBody,
                HtmlBody = e.ReceivedEmail.HtmlBody
            })
            .ToArrayAsync(ct);

        return new ManualProcessingEmailInfoCollection
        {
            Items = items
        };
    }
}