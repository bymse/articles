using JetBrains.Annotations;

namespace Collector.Application.Entities;

public class ConfirmedSource : Source
{
    public ConfirmedSource(UnconfirmedSource unconfirmed) : base(SourceState.Confirmed)
    {
        Id = unconfirmed.Id;
        CreatedAt = unconfirmed.CreatedAt;
        Receiver = new Receiver(unconfirmed.Receiver.Email);
        Title = unconfirmed.Title;
        WebPage = unconfirmed.WebPage;
        Tenant = unconfirmed.Tenant;
        ConfirmedAt = DateTimeOffset.UtcNow;
    }

    public DateTimeOffset ConfirmedAt { get; }

    [UsedImplicitly]
    protected ConfirmedSource() : base(SourceState.Confirmed)
    {
    }
}