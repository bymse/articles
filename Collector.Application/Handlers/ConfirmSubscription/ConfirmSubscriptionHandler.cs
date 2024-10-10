using Application.Extensions;
using Collector.Application.Entities;
using Collector.Application.Services;
using FluentResults;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Collector.Application.Handlers.ConfirmSubscription;

public record ConfirmSubscriptionCommand(Ulid ReceivedEmailId);

public class ConfirmSubscriptionHandler(
    ConfirmSubscriptionValidator validator,
    DbContext context,
    ILogger<ConfirmSubscriptionHandler> logger
)
{
    public async Task Handle(ConfirmSubscriptionCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);
        logger.LogInformation("Planning manual confirmation for email {ReceivedEmailId}", command.ReceivedEmailId);

        var email = await context.GetEntity<ReceivedEmail>(command.ReceivedEmailId, ct);
        var manualProcessing = ManualProcessingEmail.Confirm(email);
        context.Add(manualProcessing);

        await context.SaveChangesAsync(ct);
    }
}