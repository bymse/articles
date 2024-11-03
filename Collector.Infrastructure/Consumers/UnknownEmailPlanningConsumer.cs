using Application.Consumers;
using Collector.Application.Events;
using Collector.Application.Handlers.UnknownEmail;

namespace Collector.Infrastructure.Consumers;

public class UnknownEmailPlanningConsumer(PlanUnknownEmailManualProcessingHandler handler)
    : EventConsumer<UnknownEmailReceivedEvent>
{
    protected override Task Consume(UnknownEmailReceivedEvent @event, CancellationToken ct)
    {
        return handler.Handle(new PlanUnknownEmailManualProcessingCommand(@event.ReceivedEmailId), ct);
    }
}