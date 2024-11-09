using Bymse.Articles.PublicApi.Client;
using Bymse.Articles.Tests.Emails;

namespace Bymse.Articles.Tests.Actions;

public interface IArticlesActions
{
    ICollectorActions Collector { get; }
    IExternalSystemActions ExternalSystem { get; }
}

public interface ICollectorActions
{
    Task<UnconfirmedSourceInfo> CreateSource();
    Task<ManualProcessingEmailInfoCollection> GetManualProcessingEmails();
    Task<SourceInfo> CreateConfirmedSource();
}

public interface IExternalSystemActions
{
    Task<EmailMessage> SendConfirmationEmail(UnconfirmedSourceInfo source);
    Task<EmailMessage> SendArticlesEmail(SourceInfo source, string fileName);
}

public class ArticlesActions(ICollectorActions collectorActions, IExternalSystemActions externalSystemActions) : IArticlesActions
{
    public ICollectorActions Collector => collectorActions;
    public IExternalSystemActions ExternalSystem => externalSystemActions;
}