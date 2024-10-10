using Application.Extensions;
using Collector.Application.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Collector.Application.Handlers.UnknownEmail;

public record PlanUnknownEmailManualProcessingCommand(Ulid ReceivedEmailId);

public class PlanUnknownEmailManualProcessingHandler(
    DbContext context,
    PlanUnknownEmailManualProcessingValidator validator,
    ILogger<PlanUnknownEmailManualProcessingHandler> logger
)
{
    public async Task Handle(PlanUnknownEmailManualProcessingCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);
        
        logger.LogInformation("Unknown email received: {ReceivedEmailId}", command.ReceivedEmailId);
        
        var email = await context.GetEntity<ReceivedEmail>(command.ReceivedEmailId, ct);
        var manualProcessing = ManualProcessingEmail.UnknownEmail(email);
        context.Add(manualProcessing);
        await context.SaveChangesAsync(ct);
    }
}