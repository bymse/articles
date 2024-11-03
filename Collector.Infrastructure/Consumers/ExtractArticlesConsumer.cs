using Application.Consumers;
using Collector.Application.Events;
using Collector.Application.Handlers.ExtractArticles;
using MassTransit;

namespace Collector.Infrastructure.Consumers;

public class ExtractArticlesConsumer(SaveArticlesHandler handler) : EventConsumer<ArticlesEmailReceivedEvent>
{
    protected override async Task Consume(ArticlesEmailReceivedEvent @event, CancellationToken ct)
    {
        await handler.Handle(new SaveArticlesCommand(@event.ReceivedEmailId), ct);
    }
}