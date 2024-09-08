using Application.DbContexts;
using MassTransit;

namespace Application.Mediator;

public class UseCaseCommitFilter<T>(IUseCaseDbContextProvider dbContextProvider)
    : IFilter<SendContext<T>> where T : class, IUseCase
{
    public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
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