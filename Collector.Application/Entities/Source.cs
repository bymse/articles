using Collector.Integration;

namespace Collector.Application.Entities;

public record Receiver(string Email);

public abstract class Source(SourceState state)
{
    public const int MAX_TITLE_LENGTH = 100;
    
    public CollectorSourceId Id { get; protected set; } = new(Ulid.NewUlid());

    public SourceState State { get; protected set; } = state;

    public DateTimeOffset CreatedAt { get; protected set; }

    public Receiver Receiver { get; protected set; } = null!;

    public string Title { get; protected set; }

    public Uri WebPage { get; protected set; }
    
    public Tenant Tenant { get; protected init; }
}

public enum SourceState
{
    Unconfirmed,
    Confirmed,
}