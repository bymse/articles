using MassTransit;

namespace Application.Contexts;

public class MassTransitConsumeContextFilter<T>(ConsumeContextManager manager) : IFilter<ConsumeContext<T>>
    where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        manager.Set(context);
        try
        {
            await next.Send(context);
        }
        finally
        {
            manager.Clear();
        }
    }

    public void Probe(ProbeContext context)
    {
    }
}