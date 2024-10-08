using Application.DbContexts;
using Application.Events;
using Application.Mediator;
using Collector.Application.Entities;
using Collector.Application.Events;
using Collector.Application.Services;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Collector.Application.UseCases.ReceiveEmails;

public class ReceiveEmailsUseCase : IUseCase;

public class ReceiveEmailsHandler(
    IImapEmailService service,
    IUseCaseDbContextProvider provider,
    ILogger<ReceiveEmailsUseCase> logger,
    IEventPublisher publisher)
    : UseCaseHandler<ReceiveEmailsUseCase>
{
    protected override async Task Handle(ReceiveEmailsUseCase request, CancellationToken ct)
    {
        var dbContext = provider.GetFor<ReceiveEmailsUseCase>();
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