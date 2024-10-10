using Collector.Application.Events;
using Collector.Application.Handlers.UnknownEmail;
using MassTransit;

namespace Collector.Infrastructure.Consumers;

public class UnknownEmailPlanningConsumer(PlanUnknownEmailManualProcessingHandler handler)
    : IConsumer<UnknownEmailReceivedEvent>
{
    public Task Consume(ConsumeContext<UnknownEmailReceivedEvent> context)
    {
        return handler.Handle(
            new PlanUnknownEmailManualProcessingCommand(context.Message.ReceivedEmailId),
            context.CancellationToken
        );
    }
}