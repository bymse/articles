using MassTransit;
using Registry.Integration;

namespace Bymse.Articles.Tests.TestConsumers;

public class GenericConsumer(MessagesReceiver receiver) : IConsumer<SaveArticleTask>
{
    public Task Consume(ConsumeContext<SaveArticleTask> context)
    {
        receiver.Receive(context.Message);
        return Task.CompletedTask;
    }
}