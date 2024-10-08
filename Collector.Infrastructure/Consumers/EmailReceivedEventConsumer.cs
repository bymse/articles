using Application.Mediator;
using Collector.Application.Events;
using Collector.Application.UseCases.ProcessEmail;
using MassTransit;

namespace Collector.Infrastructure.Consumers;

public class EmailReceivedEventConsumer(ISender sender) : IConsumer<EmailReceivedEvent>
{
    public async Task Consume(ConsumeContext<EmailReceivedEvent> context)
    {
        await sender.Send(new ProcessEmailUseCase(context.Message.ReceivedEmailId), context.CancellationToken);
    }
}