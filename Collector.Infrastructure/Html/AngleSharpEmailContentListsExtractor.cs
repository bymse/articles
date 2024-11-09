using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Application.Di;
using Collector.Application.Entities;
using Collector.Application.Services;

namespace Collector.Infrastructure.Html;

[AutoRegistration]
public class AngleSharpEmailContentListsExtractor : IEmailContentListsExtractor
{
    public async Task<EmailContentList> ExtractFromHtml(string html, SourceType type, CancellationToken ct)
    {
        if (type != SourceType.BonoboEmailDigest)
        {
            throw new NotSupportedException($"Source type {type} is not supported.");
        }
        
        var htmlParser = new HtmlParser();
        var doc = await htmlParser.ParseDocumentAsync(html);

        return new EmailContentList
        {
            Elements = GetElements(doc).ToArray()
        };
    }

    private static IEnumerable<EmailContentListElement> GetElements(IHtmlDocument doc)
    {
        var items = doc.QuerySelectorAll("#content-blocks td.dd");

        for (var i = 0; i < items.Length; i+=2)
        {
            var link = items[i].QuerySelector("a");
            var description = items[i + 1].QuerySelector("p");
            
            if (link == null)
            {
                continue;
            }
            
            var href = link.GetAttribute("href");
            if (string.IsNullOrWhiteSpace(href))
            {
                continue;
            }
            
            yield return new EmailContentListElement
            {
                Title = CleanText(link.TextContent)!,
                Url = new Uri(href),
                Description = description?.TextContent
            };
        }
    }

    private static readonly Regex WhitespaceRegex = new(@"\s+", RegexOptions.Compiled);

    private static string? CleanText(string? text)
    {
        return string.IsNullOrWhiteSpace(text)
            ? null
            : WhitespaceRegex.Replace(text, " ").Trim();
    }
}