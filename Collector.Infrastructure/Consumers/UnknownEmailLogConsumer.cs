using Collector.Application.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Collector.Infrastructure.Consumers;

public class UnknownEmailLogConsumer(ILogger<UnknownEmailLogConsumer> logger) : IConsumer<UnknownEmailReceivedEvent>
{
    public Task Consume(ConsumeContext<UnknownEmailReceivedEvent> context)
    {
        logger.LogError("Received unknown email with id {ReceivedEmailId}", context.Message.ReceivedEmailId);
        
        return Task.CompletedTask;
    }
}