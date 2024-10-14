using Application.Contexts;
using Integration;
using MassTransit;

namespace Application.Events;

public interface IPublisher
{
    Task PublishEvent<TE>(TE @event,
        CancellationToken ct,
        Action<PublishContext<TE>>? config = null) where TE : class, IEvent;

    Task PublishTask<T>(T task, CancellationToken ct, Action<PublishContext<T>>? config = null) where T : class, ITask;
}

public class Publisher(IPublishEndpoint publishEndpoint, ConsumeContextManager contextManager)
    : IPublisher
{
    public Task PublishEvent<TE>(TE @event, CancellationToken ct, Action<PublishContext<TE>>? config = null)
        where TE : class, IEvent =>
        Publish(@event, ct, config);

    public Task PublishTask<T>(T task, CancellationToken ct, Action<PublishContext<T>>? config = null)
        where T : class, ITask => Publish(task, ct, config);

    private Task Publish<TE>(TE @event, CancellationToken ct, Action<PublishContext<TE>>? config = null)
        where TE : class
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