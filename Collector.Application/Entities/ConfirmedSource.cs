using JetBrains.Annotations;

namespace Collector.Application.Entities;

public class ConfirmedSource : Source
{
    public ConfirmedSource(UnconfirmedSource unconfirmed) : base(SourceState.Confirmed, unconfirmed.Type)
    {
        Id = unconfirmed.Id;
        CreatedAt = unconfirmed.CreatedAt;
        Receiver = new Receiver(unconfirmed.Receiver.Email);
        Title = unconfirmed.Title;
        WebPage = unconfirmed.WebPage;
        Tenant = unconfirmed.Tenant;
        ConfirmedAt = DateTimeOffset.UtcNow;

        if (unconfirmed.Type == null)
        {
            throw new InvalidOperationException("Type is not set for confirmed source");
        }
    }

    public DateTimeOffset ConfirmedAt { get; }

    public new SourceType Type =>
        base.Type ?? throw new InvalidOperationException("Type is not set for confirmed source");

    [UsedImplicitly]
    protected ConfirmedSource() : base(SourceState.Confirmed, null)
    {
    }
}