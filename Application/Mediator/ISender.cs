using Application.Contexts;
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

public class Sender(IScopedMediator mediator, ConsumeContextManager consumeContextProvider)
    : ISender
{
    public async Task Send(IUseCase useCase, CancellationToken ct)
    {
        var consumeContext = consumeContextProvider.Find();
        using var handle = consumeContext != null
            ? mediator.CreateRequest(consumeContext, useCase, ct, RequestTimeout.None)
            : mediator.CreateRequest(useCase, ct, RequestTimeout.None);

        var resultResponse = handle.GetResponse<VoidResponse>(false);
        var errorResponse = handle.GetResponse<FluentValidation.Results.ValidationResult>();

        var response = await Task.WhenAny(resultResponse, errorResponse);
        if (response == errorResponse)
        {
            var validationResult = (await errorResponse).Message;
            throw new ValidationException(validationResult.Errors);
        }
    }

    public async Task<TR> Send<TR>(IUseCase<TR> useCase, CancellationToken ct) where TR : class
    {
        var consumeContext = consumeContextProvider.Find();
        using var handle = consumeContext != null
            ? mediator.CreateRequest<Request<TR>>(consumeContext, useCase, ct, RequestTimeout.None)
            : mediator.CreateRequest<Request<TR>>(useCase, ct, RequestTimeout.None);

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