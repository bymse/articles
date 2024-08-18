namespace Collector.Application.Entities;

public record SourceId(Ulid Value);

public record Receiver(string Email);

public abstract class Source(SourceState state)
{
    public SourceId Id { get; } = new SourceId(Ulid.NewUlid());

    public SourceState State { get; protected set; } = state;

    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}

public class UnconfirmedSource(Uri webPage) : Source(SourceState.Unconfirmed)
{
    private Receiver? receiver;

    public Uri WebPage { get; } = webPage;

    public Receiver GenerateReceiver(string emailDomain)
    {
        if (receiver != null)
        {
            throw new InvalidOperationException("Email already generated");
        }

        receiver = new Receiver($"{Id.Value}@{emailDomain}");
        return receiver;
    }
}

public class ConfirmedSource() : Source(SourceState.Confirmed)
{
    public DateTimeOffset ConfirmedAt { get; } = DateTimeOffset.UtcNow;

    public Receiver Receiver { get; } = null!;
}

public enum SourceState
{
    Unconfirmed,
    Confirmed,
}