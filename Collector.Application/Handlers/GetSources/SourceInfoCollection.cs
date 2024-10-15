using Collector.Application.Entities;

namespace Collector.Application.Handlers.GetSources;

public class SourceInfoCollection
{
    public SourceInfo[] Items { get; init; } = [];
}

public class SourceInfo
{
    public Ulid Id { get; init; }
    public required string Title { get; init; }
    public required Uri WebPage { get; init; }
    public required SourceState State { get; init; }
    public required string ReceiverEmail { get; init; }
}