using System.Collections.Concurrent;

namespace Bymse.Articles.Tests.TestConsumers;

public class MessagesReceiver
{
    private readonly ConcurrentBag<object> receivedMessages = new();
    
    public void Receive(object message)
    {
        receivedMessages.Add(message);
    }
    
    public IEnumerable<T> GetReceivedMessages<T>()
    {
        return receivedMessages.OfType<T>();
    }
}