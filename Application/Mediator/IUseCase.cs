using MassTransit.Mediator;

namespace Application.Mediator;

public interface IUseCase
{
}

public interface IUseCase<TResult> : IUseCase, Request<TResult> where TResult : class
{
}