using Collector.Application.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.ConfirmSource;

public record ConfirmSourceCommand(string ReceiverEmail);

public class ConfirmSourceHandler(DbContext context, ConfirmSourceValidator validator)
{
    public async Task Handle(ConfirmSourceCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var source = context
            .Set<UnconfirmedSource>()
            .Local
            .Single(s => s.Receiver.Email == command.ReceiverEmail);

        var entry = context.Attach(source.Confirm());
        entry.State = EntityState.Modified;
        await context.SaveChangesAsync(ct);
    }
}