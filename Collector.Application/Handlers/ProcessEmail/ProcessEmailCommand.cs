using Application.Handlers;

namespace Collector.Application.Handlers.ProcessEmail;

public record ProcessEmailCommand(Ulid ReceivedEmailId);

public class ProcessEmailHandler : IApplicationHandler
{
    public async Task Handle(ProcessEmailCommand request, CancellationToken ct)
    {
        Console.WriteLine("Processing email with ID {0}", request.ReceivedEmailId);
    }
}