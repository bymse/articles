using Application.Consumers;
using Collector.Application.Events;
using Collector.Application.Handlers.ProcessEmail;

namespace Collector.Application.Consumers;

public class EmailReceivedEventConsumer(ProcessEmailHandler handler) : EventConsumer<EmailReceivedEvent>
{
    protected override Task Consume(EmailReceivedEvent @event, CancellationToken ct)
    {
        return handler.Handle(new ProcessEmailCommand(@event.ReceivedEmailId), ct);
    }
}