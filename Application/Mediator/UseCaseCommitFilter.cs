using Application.DbContexts;
using MassTransit;

namespace Application.Mediator;

public class UseCaseCommitFilter<T>(IUseCaseDbContextProvider dbContextProvider)
    : IFilter<ConsumeContext<T>> where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        await next.Send(context);

        var dbContext = dbContextProvider.GetFor(context.Message.GetType());
        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }

    public void Probe(ProbeContext context)
    {
    }
}