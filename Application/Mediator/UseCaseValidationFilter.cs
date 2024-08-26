using FluentValidation;
using MassTransit;

namespace Application.Mediator;

public class UseCaseValidationFilter<T>(IEnumerable<IValidator<T>> validators) : IFilter<ConsumeContext<T>>
    where T : class, IUseCase
{
    public Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        foreach (var validator in validators)
        {
            var result = validator.Validate(context.Message);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }

        return next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
    }
}