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
    EmailSubscriptionConfirmationService confirmationService,
    ILogger<ConfirmSubscriptionHandler> logger
)
{
    public async Task Handle(ConfirmSubscriptionCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);
        var email = await context.GetEntity<ReceivedEmail>(command.ReceivedEmailId, ct);

        var result = await confirmationService.TryConfirm(email);
        if (result.IsSuccess)
        {
            await ConfirmSource(email);
        }
        else
        {
            HandleFailure(email, result);
        }

        await context.SaveChangesAsync(ct);
    }

    private async Task ConfirmSource(ReceivedEmail email)
    {
        var source = await context
            .Set<UnconfirmedSource>()
            .Where(e => e.Receiver.Email == email.ToEmail)
            .SingleOrDefaultAsync();

        if (source is not null)
        {
            source.Confirm();
        }
        else
        {
            logger.LogWarning("No unconfirmed source found for email {ReceivedEmailId}", email.Id);
        }
    }

    private void HandleFailure(ReceivedEmail email, IResultBase result)
    {
        var error = string.Join(", ", result.Errors.Select(e => e.Message));
        logger.LogWarning("Failed to confirm email {ReceivedEmailId}: {Error}", email.Id, error);

        var manualProcessing = ManualProcessingEmail.FailedToConfirm(email, error);
        context.Add(manualProcessing);
    }
}