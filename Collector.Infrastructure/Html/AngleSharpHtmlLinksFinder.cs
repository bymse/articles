using AngleSharp;
using AngleSharp.Html.Parser;
using Collector.Application.Services;

namespace Collector.Infrastructure.Html;

public class AngleSharpHtmlLinksFinder : IHtmlLinksFinder
{
    public async Task<IEnumerable<HtmlLink>> FindLinks(string htmlContent)
    {
        var parser = new HtmlParser();
        var document = parser.ParseDocument(htmlContent);

        return document
            .QuerySelectorAll("a")
            .Select(element => Uri.TryCreate(element.GetAttribute("href"), UriKind.Absolute, out var uri)
                ? new HtmlLink(uri, element.TextContent)
                : null)
            .OfType<HtmlLink>();
    }
}