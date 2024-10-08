using Application.Events;
using JetBrains.Annotations;
using MassTransit;

namespace Application.Consumers;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class EventConsumer<T> : IConsumer<T> where T : class, IEvent
{
    public Task Consume(ConsumeContext<T> context)
    {
        return Consume(context.Message, context.CancellationToken);
    }

    protected abstract Task Consume(T @event, CancellationToken ct);
}