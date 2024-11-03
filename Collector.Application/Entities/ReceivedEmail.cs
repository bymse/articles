using System.Collections.Specialized;

namespace Collector.Application.Entities;

public class ReceivedEmail
{
    private readonly Dictionary<string, string> headers = new();
    
    public Ulid Id { get; private init; } = Ulid.NewUlid();
    public EmailType Type { get; init; }
    
    public uint Uid { get; init; }
    public uint UidValidity { get; init; }

    public Ulid MailboxId { get; init; }

    public required string ToEmail { get; init; }
    public required string FromEmail { get; init; }
    public required string? FromName { get; init; }
    public string? Subject { get; init; }
    public string? HtmlBody { get; init; }
    public string? TextBody { get; init; }
    public IReadOnlyDictionary<string, string> Headers => headers;

    public DateTimeOffset ReceivedAt { get; init; }

    public void SetHeaders(Dictionary<string, string> headers)
    {
        foreach (var (key, value) in headers)
        {
            this.headers[key] = value;
        }
    }
}

public enum EmailType
{
    Unknown,
    Articles,
    Confirmation
}