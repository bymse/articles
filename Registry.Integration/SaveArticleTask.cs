using Integration;

namespace Registry.Integration;

public class SaveArticleTask : ITask
{
    public required string Title { get; init; }
    public required Uri Url { get; init; }
    public required ArticleTag[] Tags { get; init; }
    public required string? Desription { get; init; }
}