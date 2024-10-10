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
    DbContext dbContext,
    EmailClassifier emailClassifier)
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
            var receivedEmail = await HandleEmail(model, mailbox, ct);
            logger.LogInformation("Received email {ReceivedEmailId}", receivedEmail.Id);

            mailbox.SetUid(model.Uid, model.UidValidity);
            dbContext.Add(receivedEmail);
            await dbContext.SaveChangesAsync(ct);
        }
    }

    private async Task<ReceivedEmail> HandleEmail(EmailModel model, Mailbox mailbox, CancellationToken ct)
    {
        var emailType = await emailClassifier.Classify(model);

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
            Type = emailType
        };
        receivedEmail.SetHeaders(model.Headers);

        switch (emailType)
        {
            case EmailType.Articles:
                await publisher.Publish(new ArticlesEmailReceivedEvent(receivedEmail.Id), ct);
                break;
            case EmailType.Confirmation:
                await publisher.Publish(new ConfirmationEmailReceivedEvent(receivedEmail.Id), ct);
                break;
            case EmailType.Unknown:
                await publisher.Publish(new UnknownEmailReceivedEvent(receivedEmail.Id), ct);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return receivedEmail;
    }
}