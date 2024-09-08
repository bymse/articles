using System.Diagnostics;
using FluentValidation;
using MassTransit;

namespace Application.Mediator;

public class UseCaseValidationFilter<T>(IEnumerable<IValidator<T>> validators) : IFilter<ConsumeContext<T>>
    where T : class
{
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var timer = Stopwatch.StartNew();
        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(context.Message, context.CancellationToken);
            timer.Stop();
            if (!result.IsValid)
            {
                await context.RespondAsync(result);
                await context.NotifyConsumed(context, timer.Elapsed, nameof(UseCaseValidationFilter<T>));
                return;
            }
        }

        await next.Send(context);
    }

    public void Probe(ProbeContext context)
    {
    }
}