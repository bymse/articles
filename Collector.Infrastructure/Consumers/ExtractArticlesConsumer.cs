using Collector.Application.Events;
using Collector.Application.Handlers.ExtractArticles;
using MassTransit;

namespace Collector.Infrastructure.Consumers;

public class ExtractArticlesConsumer(SaveArticlesHandler handler) : IConsumer<ArticlesEmailReceivedEvent>
{
    public async Task Consume(ConsumeContext<ArticlesEmailReceivedEvent> context)
    {
        await handler.Handle(
            new SaveArticlesCommand(context.Message.ReceivedEmailId),
            context.CancellationToken
        );
    }
}