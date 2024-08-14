using Identity.Infrastructure.Mediator;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Application.Mediator;

public class UseCaseCommitFilter<T>(DbContext dbContext) : IFilter<ConsumeContext<T>> where T : class, IUseCase
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        await next.Send(context);
        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }

    public void Probe(ProbeContext context)
    {
    }
}