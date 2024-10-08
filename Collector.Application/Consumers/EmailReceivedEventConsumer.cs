using Application.Consumers;
using Application.Mediator;
using Collector.Application.Events;
using Collector.Application.UseCases.ProcessEmail;

namespace Collector.Application.Consumers;

public class EmailReceivedEventConsumer(ISender sender) : EventConsumer<EmailReceivedEvent>
{
    protected override Task Consume(EmailReceivedEvent @event, CancellationToken ct)
    {
        return sender.Send(new ProcessEmailUseCase(@event.ReceivedEmailId), ct);
    }
}