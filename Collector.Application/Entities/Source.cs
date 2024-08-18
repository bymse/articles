using Collector.Integration;

namespace Collector.Application.Entities;

public record Receiver(string Email);

public abstract class Source(SourceState state)
{
    public Receiver Receiver { get; protected set; } = null!;

    public CollectorSourceId Id { get; } = new CollectorSourceId(Ulid.NewUlid());

    public SourceState State { get; protected set; } = state;

    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}

public class UnconfirmedSource : Source
{
    public UnconfirmedSource(Uri webPage, string domain) : base(SourceState.Unconfirmed)
    {
        WebPage = webPage;
        Receiver = new Receiver($"{Id.Value}@{domain}");
    }

    public Uri WebPage { get; }
}

public class ConfirmedSource() : Source(SourceState.Confirmed)
{
    public DateTimeOffset ConfirmedAt { get; } = DateTimeOffset.UtcNow;
}

public enum SourceState
{
    Unconfirmed,
    Confirmed,
}