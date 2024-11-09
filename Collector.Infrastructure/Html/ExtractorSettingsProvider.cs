using Collector.Application.Entities;

namespace Collector.Infrastructure.Html;

public record ExtractorSettings(string BlockQuery, string TitleQuery, string UrlQuery, string DescriptionQuery);

public static class ExtractorSettingsProvider
{
    public static ExtractorSettings GetSettings(SourceType type)
    {
        return type switch
        {
            SourceType.BonoboEmailDigest => new ExtractorSettings("div", "a", "a", "p"),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}