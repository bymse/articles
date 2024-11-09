using Collector.Application.Entities;

namespace Bymse.Articles.Apis.Public.Sources.Models;

public class CreateSourceRequest
{
    public string Title { get; init; } = null!;
    public Uri WebPage { get; init; } = null!;
    public SourceType Type { get; init; }
}