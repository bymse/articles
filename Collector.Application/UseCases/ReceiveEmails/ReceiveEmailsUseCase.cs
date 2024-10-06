using Application.DbContexts;
using Application.Mediator;
using Collector.Application.Entities;
using Collector.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Collector.Application.UseCases.ReceiveEmails;

public class ReceiveEmailsUseCase : IUseCase;

public class ReceiveEmailsHandler(
    IImapEmailService service,
    IUseCaseDbContextProvider provider,
    ILogger<ReceiveEmailsUseCase> logger)
    : UseCaseHandler<ReceiveEmailsUseCase>
{
    protected override async Task Handle(ReceiveEmailsUseCase request, CancellationToken ct)
    {
        var dbContext = provider.GetFor<ReceiveEmailsUseCase>();
        var mailbox = await dbContext.Set<Mailbox>().SingleOrDefaultAsync(ct) ?? new Mailbox();

        await foreach (var email in service.GetMessages(mailbox.UidValidity, mailbox.LastUid, ct))
        {
            logger.LogInformation("Received email with UID {Uid} for {ToEmail}", email.Uid, email.ToEmail);
            
            //todo: publish email to message broker
            
            mailbox.SetUid(email.Uid, email.UidValidity);
            await dbContext.SaveChangesAsync(ct);
        }
    }
}