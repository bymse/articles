namespace Collector.Application.Services;

public interface IImapEmailService
{
    IAsyncEnumerable<EmailModel> GetMessages(uint? uidValidity,
        uint? lastUid,
        int count,
        CancellationToken ct);
}

public class EmailModel
{
    public required string ToEmail { get; init; }
    public required string Subject { get; init; }
    public required string FromEmail { get; init; }
    public required string? FromName { get; init; }
    public string? TextBody { get; init; }
    public string? HtmlBody { get; init; }
    public DateTimeOffset Date { get; init; }
    public Dictionary<string, string> Headers { get; init; } = new();
    
    public uint Uid { get; init; }
    public uint UidValidity { get; init; }
}