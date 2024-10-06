namespace Collector.Application.Entities;

public class ReceivedEmail
{
    public Ulid Id { get; private init; } = Ulid.NewUlid();

    public uint Uid { get; init; }
    public uint UidValidity { get; init; }

    public Ulid MailboxId { get; init; }

    public required string ToEmail { get; init; }
    public required string FromEmail { get; init; }
    public string? Subject { get; init; }
    public string? HtmlBody { get; init; }
    public string? TextBody { get; init; }

    public DateTimeOffset ReceivedAt { get; init; }
}