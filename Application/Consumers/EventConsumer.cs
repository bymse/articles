using Application.Contexts;
using Application.Events;
using JetBrains.Annotations;
using MassTransit;

namespace Application.Consumers;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class EventConsumer<T> : IConsumer<T> where T : class, IEvent
{
    public async Task Consume(ConsumeContext<T> context)
    {
        var manager = context.GetServiceOrCreateInstance<ConsumeContextManager>();
        manager.Set(context);
        try
        {
            await Consume(context.Message, context.CancellationToken);
        }
        finally
        {
            manager.Clear();
        }
    }

    protected abstract Task Consume(T @event, CancellationToken ct);
}