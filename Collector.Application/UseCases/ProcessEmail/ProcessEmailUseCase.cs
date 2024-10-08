using Application.Mediator;

namespace Collector.Application.UseCases.ProcessEmail;

public record ProcessEmailUseCase(Ulid ReceivedEmailId) : IUseCase;

public class ProcessEmailHandler() : UseCaseHandler<ProcessEmailUseCase>
{
    protected override async Task Handle(ProcessEmailUseCase request, CancellationToken ct)
    {
        Console.WriteLine("Processing email with ID {0}", request.ReceivedEmailId);
    }
}