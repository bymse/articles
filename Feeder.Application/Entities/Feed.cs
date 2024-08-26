using Collector.Integration;
using Identity.Integration;

namespace Feeder.Application.Entities;

public record FeedId(Ulid Value);

public class Feed
{
    public const int MaxTitleLength = 100;

    private readonly HashSet<FeedSource> sources = new();

    public Feed(IdentityUserId userId, string title)
    {
        UserId = userId;
        SetTitle(title);
    }

    public FeedId Id { get; protected set; } = new(Ulid.NewUlid());

    public IdentityUserId UserId { get; protected set; }

    public string Title { get; protected set; } = null!;

    public IReadOnlyCollection<FeedSource> Sources => sources;

    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Feed title cannot be empty.", nameof(title));
        }

        if (title.Length > MaxTitleLength)
        {
            throw new ArgumentException($"Feed title cannot be longer than {MaxTitleLength} characters.",
                nameof(title));
        }

        Title = title;
    }

    public void AddSource(CollectorSourceId sourceId)
    {
        sources.Add(new FeedSource(Id, sourceId));
    }
}

public record FeedSource(FeedId FeedId, CollectorSourceId SourceId);