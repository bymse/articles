using JetBrains.Annotations;

namespace Collector.Application.Entities;

public class UnconfirmedSource : Source
{
    public UnconfirmedSource(string title, Uri webPage, string receiverEmail, Tenant tenant, SourceType type) : base(
        SourceState.Unconfirmed, type)
    {
        WebPage = webPage;
        Title = title;
        CreatedAt = DateTimeOffset.UtcNow;
        Tenant = tenant;

        var parts = receiverEmail.Split('@');
        Receiver = new Receiver($"{parts[0]}+{Id.Value}@{parts[1]}");
    }

    public ConfirmedSource Confirm() => new(this);

    [UsedImplicitly]
    private UnconfirmedSource() : base(SourceState.Unconfirmed, null)
    {
    }
}