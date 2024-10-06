using Application.Events;

namespace Collector.Application.Events;

public class EmailReceivedEvent : IEvent
{
    public Ulid ReceivedEmailId { get; init; }
}