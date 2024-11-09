using Collector.Application.Entities;

namespace Collector.Application.Services;

public class EmailContentList
{
    public required IReadOnlyList<EmailContentListElement> Elements { get; init; }
}

public class EmailContentListElement
{
    public required Uri Url { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
}

public interface IEmailContentListsExtractor
{
    IAsyncEnumerable<EmailContentList> ExtractFromHtml(string html, SourceType type);
}