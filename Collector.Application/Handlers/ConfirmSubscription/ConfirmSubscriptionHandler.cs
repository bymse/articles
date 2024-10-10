using Collector.Application.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.ConfirmSubscription;

public record ConfirmSubscriptionCommand(Ulid ReceivedEmailId);

public class ConfirmSubscriptionValidator : AbstractValidator<ConfirmSubscriptionCommand>
{
    public ConfirmSubscriptionValidator(DbContext context)
    {
        RuleFor(x => x.ReceivedEmailId)
            .MustAsync(async (e, ct) =>
            {
                var email = await context.Set<ReceivedEmail>().FindAsync([e], ct);
                return email?.Type == EmailType.Confirmation;
            });
    }
}

public class ConfirmSubscriptionHandler(ConfirmSubscriptionValidator validator, DbContext context)
{
    public async Task Handle(ConfirmSubscriptionCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);
        var email = await context.Set<ReceivedEmail>().FindAsync([command.ReceivedEmailId], ct);
        
        
    }
}