namespace Collector.Application.Services;

public record HtmlLink(Uri Url, string Text);

public interface IHtmlLinksFinder
{
    Task<IEnumerable<HtmlLink>> FindLinks(string htmlContent);
}