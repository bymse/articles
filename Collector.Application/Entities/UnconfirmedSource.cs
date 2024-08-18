using JetBrains.Annotations;

namespace Collector.Application.Entities;

public class UnconfirmedSource : Source
{
    public UnconfirmedSource(string title, Uri webPage, string domain) : base(SourceState.Unconfirmed)
    {
        WebPage = webPage;
        Title = title;
        Receiver = new Receiver($"{Id.Value}@{domain}");
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public ConfirmedSource Confirm() => new(this);

    [UsedImplicitly]
    private UnconfirmedSource() : base(SourceState.Unconfirmed)
    {
    }
}