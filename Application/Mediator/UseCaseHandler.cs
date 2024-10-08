using JetBrains.Annotations;
using MassTransit;
using MassTransit.Mediator;

namespace Application.Mediator;

public class VoidResponse();

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class UseCaseHandler<TUseCase> : IConsumer<TUseCase> where TUseCase : class, IUseCase
{
    public async Task Consume(ConsumeContext<TUseCase> context)
    {
        await Handle(context.Message, context.CancellationToken);
        await context.RespondAsync(new VoidResponse());
    }

    protected abstract Task Handle(TUseCase request, CancellationToken cancellationToken);
}

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class UseCaseHandler<TUseCase, TResult> : MediatorRequestHandler<TUseCase, TResult>
    where TUseCase : class, IUseCase<TResult> where TResult : class;