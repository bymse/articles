using Application.Contexts;
using MassTransit;

namespace Application.Events;

public interface IEventPublisher
{
    Task Publish<TE>(TE @event,
        CancellationToken ct,
        Action<PublishContext<TE>>? config = null) where TE : class, IEvent;
}

public class EventPublisher(IPublishEndpoint publishEndpoint, ConsumeContextManager contextManager) : IEventPublisher
{
    public Task Publish<TE>(TE @event, CancellationToken ct, Action<PublishContext<TE>>? config = null)
        where TE : class, IEvent
    {
        var consumeContext = contextManager.Find();

        return publishEndpoint.Publish(@event, sendContext =>
            {
                if (consumeContext != null) sendContext.TransferConsumeContextHeaders(consumeContext);
                config?.Invoke(sendContext);
            },
            ct);
    }
}