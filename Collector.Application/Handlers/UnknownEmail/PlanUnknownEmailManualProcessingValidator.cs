using Application.Extensions;
using Collector.Application.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.UnknownEmail;

public class PlanUnknownEmailManualProcessingValidator : AbstractValidator<PlanUnknownEmailManualProcessingCommand>
{
    public PlanUnknownEmailManualProcessingValidator(DbContext context)
    {
        RuleFor(c => c.ReceivedEmailId)
            .MustAsync(async (id, ct) =>
            {
                var email = await context.FindEntity<ReceivedEmail>(id, ct);
                return email?.Type == EmailType.Unknown;
            });
    }
}