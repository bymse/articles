using Application.Events;
using Collector.Application.Entities;
using Collector.Application.Events;
using Collector.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Collector.Application.Handlers.ReceiveEmails;

public class ReceiveEmailsHandler(
    IImapEmailService service,
    ILogger<ReceiveEmailsHandler> logger,
    IEventPublisher publisher,
    DbContext dbContext)
{
    public async Task Handle(CancellationToken ct)
    {
        var mailbox = await dbContext.Set<Mailbox>().SingleOrDefaultAsync(ct);
        if (mailbox == null)
        {
            mailbox = new Mailbox();
            dbContext.Add(mailbox);
            await dbContext.SaveChangesAsync(ct);
        }

        await foreach (var model in service.GetMessages(mailbox.UidValidity, mailbox.LastUid, ct))
        {
            logger.LogInformation("Received email with UID {Uid} for {ToEmail}", model.Uid, model.ToEmail);

            var receivedEmail = new ReceivedEmail
            {
                ToEmail = model.ToEmail,
                Subject = model.Subject,
                FromEmail = model.FromEmail,
                ReceivedAt = DateTimeOffset.UtcNow,
                TextBody = model.TextBody,
                HtmlBody = model.HtmlBody,
                UidValidity = model.UidValidity,
                Uid = model.Uid,
                MailboxId = mailbox.Id,
            };
            dbContext.Add(receivedEmail);

            await publisher.PublishEvent(new EmailReceivedEvent
            {
                ReceivedEmailId = receivedEmail.Id
            });

            mailbox.SetUid(model.Uid, model.UidValidity);
            await dbContext.SaveChangesAsync(ct);
        }
    }
}