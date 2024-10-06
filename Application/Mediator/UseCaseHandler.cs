using JetBrains.Annotations;
using MassTransit;
using MassTransit.Mediator;

namespace Application.Mediator;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class UseCaseHandler<TUseCase> : IConsumer<TUseCase> where TUseCase : class, IUseCase
{
    public async Task Consume(ConsumeContext<TUseCase> context)
    {
        PublishEndpoint = context;
        await Handle(context.Message, context.CancellationToken);
        PublishEndpoint = null!;
    }

    protected IPublishEndpoint PublishEndpoint { get; private set; } = null!;

    protected abstract Task Handle(TUseCase request, CancellationToken cancellationToken);
}

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class UseCaseHandler<TUseCase, TResult> :
    IConsumer<TUseCase>
    where TUseCase : class, Request<TResult>
    where TResult : class
{
    public async Task Consume(ConsumeContext<TUseCase> context)
    {
        PublishEndpoint = context;
        var response = await Handle(context.Message, context.CancellationToken).ConfigureAwait(false);

        await context.RespondAsync(response).ConfigureAwait(false);
        PublishEndpoint = null!;
    }

    protected IPublishEndpoint PublishEndpoint { get; private set; } = null!;

    protected abstract Task<TResult> Handle(TUseCase request, CancellationToken cancellationToken);
}