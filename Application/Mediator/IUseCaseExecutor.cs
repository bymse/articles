using MassTransit.Mediator;

namespace Application.Mediator;

public interface IUseCaseExecutor
{
    Task Execute(IUseCase useCase, CancellationToken ct);

    Task<TR> Execute<TR>(IUseCase<TR> useCase, CancellationToken ct) where TR : class;
}

public class UseCaseExecutor : IUseCaseExecutor
{
    private readonly IScopedMediator mediator;

    public UseCaseExecutor(IScopedMediator mediator)
    {
        this.mediator = mediator;
    }

    public Task Execute(IUseCase useCase, CancellationToken ct)
    {
        return mediator.Send(useCase, ct);
    }

    public async Task<TR> Execute<TR>(IUseCase<TR> useCase, CancellationToken ct) where TR : class
    {
        var client = mediator.CreateRequestClient<IUseCase<TR>>();
        var response = await client.GetResponse<TR>(useCase, ct);

        return response.Message;
    }
}