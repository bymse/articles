using MassTransit;

namespace Application.Mediator;

public abstract class UseCaseHandler<TUseCase> : IConsumer<TUseCase> where TUseCase : class, IUseCase
{
    public abstract Task Handle(TUseCase useCase, CancellationToken ct);

    public Task Consume(ConsumeContext<TUseCase> context)
    {
        return Handle(context.Message, context.CancellationToken);
    }
}

public abstract class UseCaseHandler<TUseCase, TResult> : IConsumer<TUseCase> where TUseCase : class, IUseCase<TResult>
{
    public async Task Consume(ConsumeContext<TUseCase> context)
    {
        var result = await Handle(context.Message, context.CancellationToken);
        await context.RespondAsync(result!);
    }

    public abstract Task<TResult> Handle(TUseCase useCase, CancellationToken ct);
}