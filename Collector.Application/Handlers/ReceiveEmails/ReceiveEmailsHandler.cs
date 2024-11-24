using System.Diagnostics;
using Application.Events;
using Collector.Application.Entities;
using Collector.Application.Events;
using Collector.Application.Services;
using Collector.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Collector.Application.Handlers.ReceiveEmails;

public class ReceiveEmailsHandler(
    IImapEmailService service,
    ILogger<ReceiveEmailsHandler> logger,
    IPublisher publisher,
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

        while (!ct.IsCancellationRequested)
        {
            using var _ = CollectorActivitySource.Source.StartActivity("ReceiveEmails", ActivityKind.Internal, null);

            var receivedCount = 0;
            const int count = 10;
            await foreach (var model in service.GetMessages(mailbox.UidValidity, mailbox.LastUid, count, ct))
            {
                var receivedEmail = await HandleEmail(model, mailbox, ct);
                logger.LogInformation("Received email {ReceivedEmailId} for {ToEmail}", receivedEmail.Id,
                    receivedEmail.ToEmail);

                mailbox.SetLastUid(model.Uid, model.UidValidity);
                dbContext.Add(receivedEmail);
                await Save(mailbox, ct);
                receivedCount++;
            }

            if (receivedCount < count)
            {
                await Task.Delay(5000, ct);
            }
        }
    }

    private async Task<ReceivedEmail> HandleEmail(EmailModel model, Mailbox mailbox, CancellationToken ct)
    {
        var emailType = await emailClassifier.Classify(model, ct);

        var receivedEmail = new ReceivedEmail
        {
            ToEmail = model.ToEmail,
            Subject = model.Subject,
            FromEmail = model.FromEmail,
            FromName = model.FromName,
            ReceivedAt = DateTimeOffset.UtcNow,
            TextBody = model.TextBody?.Trim(),
            HtmlBody = model.HtmlBody?.Trim(),
            UidValidity = model.UidValidity,
            Uid = model.Uid,
            MailboxId = mailbox.Id,
            Type = emailType
        };
        receivedEmail.SetHeaders(model.Headers);

        switch (emailType)
        {
            case EmailType.Articles:
                await publisher.PublishEvent(new ArticlesEmailReceivedEvent(receivedEmail.Id), ct);
                break;
            case EmailType.Confirmation:
                await publisher.PublishEvent(new ConfirmationEmailReceivedEvent(receivedEmail.Id), ct);
                break;
            case EmailType.Unknown:
                await publisher.PublishEvent(new UnknownEmailReceivedEvent(receivedEmail.Id), ct);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return receivedEmail;
    }

    private async Task Save(Mailbox mailbox, CancellationToken ct)
    {
        await dbContext.SaveChangesAsync(ct);
        dbContext.ChangeTracker.Clear();
        dbContext.Attach(mailbox);
    }
}