using Collector.Integration;

namespace Collector.Application.Entities;

public record Receiver(string Email);

public abstract class Source(SourceState state, SourceType? type)
{
    public const int MAX_TITLE_LENGTH = 100;
    
    public CollectorSourceId Id { get; protected set; } = new(Ulid.NewUlid());

    public SourceState State { get; protected set; } = state;

    public DateTimeOffset CreatedAt { get; protected set; }

    public Receiver Receiver { get; protected set; } = null!;

    public string Title { get; protected set; } = null!;

    public Uri WebPage { get; protected set; } = null!;

    public Tenant Tenant { get; protected init; } = null!;
    
    public SourceType? Type { get; protected init; } = type;
}

public enum SourceState
{
    Unconfirmed = 0,
    Confirmed = 1,
}

public enum SourceType
{
    BonoboEmailDigest,
}