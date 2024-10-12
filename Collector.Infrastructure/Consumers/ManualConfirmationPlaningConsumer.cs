using Collector.Application.Events;
using Collector.Application.Handlers.ConfirmSubscription;
using MassTransit;

namespace Collector.Infrastructure.Consumers;

public class ManualConfirmationPlaningConsumer(PlanManualConfirmationHandler handler)
    : IConsumer<ConfirmationEmailReceivedEvent>
{
    public async Task Consume(ConsumeContext<ConfirmationEmailReceivedEvent> context)
    {
        await handler.Handle(
            new PlanManualConfirmationCommand(context.Message.ReceivedEmailId),
            context.CancellationToken
        );
    }
}