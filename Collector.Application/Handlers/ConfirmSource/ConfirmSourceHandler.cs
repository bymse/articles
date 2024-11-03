using Application.Extensions;
using Collector.Application.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.ConfirmSource;

public record ConfirmSourceCommand(Ulid ReceivedEmailId);

public class ConfirmSourceHandler(DbContext context, ConfirmSourceValidator validator)
{
    public async Task Handle(ConfirmSourceCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);
        
        var manualProcessingEmail = await context
            .Set<ManualProcessingEmail>()
            .SingleOrDefaultAsync(e => e.ReceivedEmailId == command.ReceivedEmailId, ct);

        manualProcessingEmail?.Process();

        var email = await context.GetEntity<ReceivedEmail>(command.ReceivedEmailId, ct);

        var source = context
            .Set<UnconfirmedSource>()
            .Local
            .Single(s => s.Receiver.Email == email.ToEmail);

        var entry = context.Attach(source.Confirm());
        entry.State = EntityState.Modified;
        await context.SaveChangesAsync(ct);
    }
}