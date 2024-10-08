using Application.Contexts;
using MassTransit;

namespace Application.Events;

public interface IEventPublisher
{
    Task PublishEvent<TE>(
        TE @event,
        Action<PublishContext<TE>>? config = null
    ) where TE : class, IEvent;
}

public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint publishEndpoint;
    private readonly IConsumeContextProvider consumeContextProvider;

    public EventPublisher(IPublishEndpoint publishEndpoint, IConsumeContextProvider consumeContextProvider)
    {
        this.publishEndpoint = publishEndpoint;
        this.consumeContextProvider = consumeContextProvider;
    }

    public Task PublishEvent<TE>(TE @event, Action<PublishContext<TE>>? config = null) where TE : class, IEvent
    {
        var consumeContext = consumeContextProvider.Find();
        if (consumeContext == null)
        {
            throw new InvalidOperationException("Cannot publish event outside of use case handler");
        }

        return publishEndpoint.Publish(@event, sendContext =>
            {
                sendContext.TransferConsumeContextHeaders(consumeContext);
                config?.Invoke(sendContext);
            },
            consumeContext.CancellationToken);
    }
}