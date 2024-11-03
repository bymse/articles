using Bymse.Articles.PublicApi.Client;

namespace Bymse.Articles.Tests.Actions;

public interface IArticlesActions
{
    ICollectorActions Collector { get; }
}

public interface ICollectorActions
{
    Task<UnconfirmedSourceInfo> CreateSource();
}

public class ArticlesActions(ICollectorActions collectorActions) : IArticlesActions
{
    public ICollectorActions Collector => collectorActions;
}