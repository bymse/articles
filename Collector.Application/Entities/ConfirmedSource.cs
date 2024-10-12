using JetBrains.Annotations;

namespace Collector.Application.Entities;

public class ConfirmedSource : Source
{
    public ConfirmedSource(UnconfirmedSource unconfirmed) : base(SourceState.Confirmed)
    {
        Id = unconfirmed.Id;
        CreatedAt = unconfirmed.CreatedAt;
        Receiver = unconfirmed.Receiver;
        Title = unconfirmed.Title;
        WebPage = unconfirmed.WebPage;
        ConfirmedAt = DateTimeOffset.UtcNow;
    }

    public DateTimeOffset ConfirmedAt { get; }

    [UsedImplicitly]
    protected ConfirmedSource() : base(SourceState.Confirmed)
    {
    }
}