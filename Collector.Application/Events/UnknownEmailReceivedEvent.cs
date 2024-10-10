using Application.Events;

namespace Collector.Application.Events;

public class UnknownEmailReceivedEvent(Ulid receivedEmailId) : IEvent
{
    public Ulid ReceivedEmailId { get; } = receivedEmailId;
}