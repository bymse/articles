namespace Bymse.Articles.BFFs.PublicApi.Sources;

public class CreateSourceRequest
{
    public string Title { get; init; } = null!;
    public Uri WebPage { get; init; } = null!;
}