using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Collector.Application.Services;

namespace Collector.Infrastructure.Html;

public class AngleSharpEmailContentListsExtractor : IEmailContentListsExtractor
{
    public async IAsyncEnumerable<EmailContentList> ExtractFromHtml(string html)
    {
        var htmlParser = new HtmlParser();
        var doc = await htmlParser.ParseDocumentAsync(html);
        var links = doc.QuerySelectorAll("a");
        var processedLinks = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var link in links)
        {
            var href = link.GetAttribute("href");
            if (string.IsNullOrWhiteSpace(href))
            {
                continue;
            }

            if (processedLinks.Contains(href))
            {
                continue;
            }

            var result = SimilarParentLinksSearcher.Search(link);
            if (result == SearchResult.Empty)
            {
                continue;
            }

            yield return new EmailContentList
            {
                Header = FindHeader(result.CommonParent),
                Elements = BuildElements(result.Links, processedLinks)
            };
        }
    }

    private static string? FindHeader(IElement parent)
    {
        var content = parent.QuerySelector("h1, h2, h3, h4, h5, h6")?.TextContent;
        return CleanText(content);
    }

    private static IReadOnlyList<EmailContentListElement> BuildElements(
        IReadOnlyList<IElement> links,
        HashSet<string> processedLinks
    )
    {
        var elements = new List<EmailContentListElement>();
        foreach (var link in links)
        {
            var href = link.GetAttribute("href");
            if (string.IsNullOrWhiteSpace(href))
            {
                continue;
            }

            if (!processedLinks.Add(href))
            {
                continue;
            }

            if (!Uri.TryCreate(href, UriKind.Absolute, out var uri))
            {
                continue;
            }

            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            {
                continue;
            }

            var element = new EmailContentListElement
            {
                Url = uri,
                Title = CleanText(link.TextContent) ?? uri.ToString(),
            };

            elements.Add(element);
        }

        return elements;
    }

    private static readonly Regex WhitespaceRegex = new(@"\s+", RegexOptions.Compiled);

    private static string? CleanText(string? text)
    {
        return string.IsNullOrWhiteSpace(text)
            ? null
            : WhitespaceRegex.Replace(text, " ").Trim();
    }
}