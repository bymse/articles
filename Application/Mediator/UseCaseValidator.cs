using FluentValidation;
using JetBrains.Annotations;

namespace Application.Mediator;

[UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
public abstract class UseCaseValidator<T> : AbstractValidator<T>
{
    
}