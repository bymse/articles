namespace Collector.Application.Services;

public interface IImapEmailService
{
    IAsyncEnumerable<EmailModel> GetMessages(
        uint? uidValidity,
        uint? lastUid,
        CancellationToken ct
    );
}

public class EmailModel
{
    public required string ToEmail { get; init; }
    public required string Subject { get; init; }
    public required string FromEmail { get; init; }
    public required string Body { get; init; }
    public DateTimeOffset Date { get; init; }
    
    public uint Uid { get; init; }
    public uint UidValidity { get; init; }
}