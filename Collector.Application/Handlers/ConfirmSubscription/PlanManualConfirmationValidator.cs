using Application.Extensions;
using Collector.Application.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.ConfirmSubscription;

public class PlanManualConfirmationValidator : AbstractValidator<PlanManualConfirmationCommand>
{
    public PlanManualConfirmationValidator(DbContext context)
    {
        RuleFor(x => x.ReceivedEmailId)
            .MustAsync(async (id, ct) =>
            {
                var email = await context.FindEntity<ReceivedEmail>(id, ct);
                return email?.Type == EmailType.Confirmation;
            });
    }
}