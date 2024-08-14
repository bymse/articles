namespace Identity.Infrastructure.Mediator;

public interface IUseCaseExecutor
{
    Task Execute(IUseCase useCase, CancellationToken ct);
    Task<T> Execute<T>(IUseCase<T> useCase, CancellationToken ct);
}