using Application.Consumers;
using Collector.Application.Events;
using Collector.Application.Handlers.ConfirmSubscription;

namespace Collector.Infrastructure.Consumers;

public class ManualConfirmationPlanningConsumer(PlanManualConfirmationHandler handler)
    : EventConsumer<ConfirmationEmailReceivedEvent>
{
    protected override async Task Consume(ConfirmationEmailReceivedEvent @event, CancellationToken ct)
    {
        await handler.Handle(new PlanManualConfirmationCommand(@event.ReceivedEmailId), ct);
    }
}