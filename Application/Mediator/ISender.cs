using FluentValidation;
using MassTransit;
using MassTransit.Mediator;
using MassTransit.Middleware;

namespace Application.Mediator;

public interface ISender
{
    Task Send(IUseCase useCase, CancellationToken ct);
    Task<TR> Send<TR>(IUseCase<TR> useCase, CancellationToken ct) where TR : class;
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

    public async Task<TR> Send<TR>(IUseCase<TR> useCase, CancellationToken ct) where TR : class
    {
        using var handle = mediator.CreateRequest<Request<TR>>(useCase, ct);
        var resultResponse = handle.GetResponse<TR>(false);
        var errorResponse = handle.GetResponse<FluentValidation.Results.ValidationResult>();

        var response = await Task.WhenAny(resultResponse, errorResponse);
        if (response == errorResponse)
        {
            
            var validationResult = (await errorResponse).Message;
            throw new ValidationException(validationResult.Errors);
        }

        return (await resultResponse).Message;
    }
}