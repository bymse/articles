using Application.Extensions;
using Collector.Application.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Collector.Application.Handlers.ConfirmSubscription;

public record PlanManualConfirmationCommand(Ulid ReceivedEmailId);

public class PlanManualConfirmationHandler(
    PlanManualConfirmationValidator validator,
    DbContext context,
    ILogger<PlanManualConfirmationHandler> logger
)
{
    public async Task Handle(PlanManualConfirmationCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);
        logger.LogInformation("Planning manual confirmation for email {ReceivedEmailId}", command.ReceivedEmailId);

        var email = await context.GetEntity<ReceivedEmail>(command.ReceivedEmailId, ct);
        var manualProcessing = ManualProcessingEmail.Confirm(email);
        context.Add(manualProcessing);

        await context.SaveChangesAsync(ct);
    }
}