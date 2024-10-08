using MassTransit;

namespace Application.Contexts;

public class ConsumeContextManager : IConsumeContextProvider
{
    private ConsumeContext? consumeContext;

    public void Set(ConsumeContext context)
    {
        consumeContext = context;
    }

    public ConsumeContext? Find()
    {
        return consumeContext;
    }

    public void Clear()
    {
        consumeContext = null;
    }
}

public interface IConsumeContextProvider
{
    ConsumeContext? Find();
}