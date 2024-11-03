using Collector.Application.Entities;

namespace Collector.Application.Handlers.GetManualProcessingEmails;

public class ManualProcessingEmailInfoCollection
{
    public ManualProcessingEmailInfo[] Items { get; init; } = [];
}

public class ManualProcessingEmailInfo
{
    public required Ulid ReceivedEmailId { get; init; }
    public required ManualProcessingEmailType Type { get; init; }

    public required string ToEmail { get; init; }
    public required string FromEmail { get; init; }
    public required string? FromName { get; init; }
    public required string? Subject { get; init; }
    public required string? TextBody { get; init; }
    public required string? HtmlBody { get; init; }
}