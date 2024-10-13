using AngleSharp.Dom;

namespace Collector.Infrastructure.Html;

public record SearchResult(IElement CommonParent, IReadOnlyList<IElement> Links)
{
    public static SearchResult Empty => new(null!, Array.Empty<IElement>());
}

public static class SimilarParentLinksSearcher
{
    public static SearchResult Search(IElement firstLink)
    {
        var parent = firstLink.ParentElement;
        while (parent != null)
        {
            var links = parent
                .QuerySelectorAll("a")
                .Where(e => e != firstLink)
                .ToArray();

            if (links.Length == 0)
            {
                parent = parent.ParentElement;
                continue;
            }

            var withSimilarParents = links
                .TakeWhile(link => HasSimilarParents(firstLink, link, parent))
                .ToArray();

            return withSimilarParents.Length > 0
                ? new SearchResult(parent, withSimilarParents.Prepend(firstLink).ToArray())
                : SearchResult.Empty;
        }

        return SearchResult.Empty;
    }

    private static bool HasSimilarParents(IElement firstLink, IElement linkToCheck, IElement knownParent)
    {
        var parentOfFirst = firstLink.ParentElement;
        var parentOfSecond = linkToCheck.ParentElement;
        while (parentOfFirst != null && parentOfSecond != null)
        {
            if (parentOfFirst == knownParent && parentOfSecond == knownParent)
            {
                return true;
            }

            if (!AreSimilar(parentOfFirst, parentOfSecond))
            {
                return false;
            }

            parentOfFirst = parentOfFirst.ParentElement;
            parentOfSecond = parentOfSecond.ParentElement;
        }

        return false;
    }

    private static bool AreSimilar(IElement first, IElement? second)
    {
        return first.TagName == second?.TagName && first.ClassName == second.ClassName;
    }
}