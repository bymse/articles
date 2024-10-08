using MassTransit;
using MassTransit.Mediator;

namespace Application.Mediator;

[ExcludeFromTopology]
[ExcludeFromImplementedTypes]
public interface IUseCase
{
}

[ExcludeFromTopology]
[ExcludeFromImplementedTypes]
public interface IUseCase<TResult> : IUseCase, Request<TResult> where TResult : class
{
}