using JetBrains.Annotations;
using MassTransit.Mediator;

namespace Application.Mediator;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class UseCaseHandler<TUseCase> : MediatorRequestHandler<TUseCase> where TUseCase : class, IUseCase;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class UseCaseHandler<TUseCase, TResult> : MediatorRequestHandler<TUseCase, TResult>
    where TUseCase : class, IUseCase<TResult> where TResult : class;