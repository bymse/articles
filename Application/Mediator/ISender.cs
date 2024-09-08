using MassTransit;
using MassTransit.Mediator;

namespace Application.Mediator;

public interface ISender
{
    Task Send(IUseCase useCase, CancellationToken ct);
    Task<TR> SendRequest<TR>(IUseCase<TR> useCase, CancellationToken ct) where TR : class;
}

public class Sender : ISender
{
    private readonly IScopedMediator mediator;

    public Sender(IScopedMediator mediator)
    {
        this.mediator = mediator;
    }

    public Task Send(IUseCase useCase, CancellationToken ct)
    {
        return mediator.Send(useCase, useCase.GetType(), ct);
    }

    public Task<TR> SendRequest<TR>(IUseCase<TR> useCase, CancellationToken ct) where TR : class
    {
        return mediator.SendRequest(useCase, ct);
    }
}