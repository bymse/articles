﻿using System.Text.RegularExpressions;
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
        var htmlParser = new HtmlParser();
        var doc = await htmlParser.ParseDocumentAsync(html);
        var settings = ExtractorSettingsProvider.GetSettings(type);

        return new EmailContentList
        {
            Elements = GetElements(doc, settings).ToArray()
        };
    }

    private static IEnumerable<EmailContentListElement> GetElements(IHtmlDocument doc, ExtractorSettings settings)
    {
        var items = doc.QuerySelectorAll(settings.BlockQuery);

        foreach (var element in items)
        {
            var title = element.QuerySelector(settings.TitleQuery);
            var url = element.QuerySelector(settings.UrlQuery);
            var description = element.QuerySelector(settings.DescriptionQuery);

            if (title?.TextContent == null || url == null)
            {
                continue;
            }

            var href = url.GetAttribute("href");
            if (string.IsNullOrWhiteSpace(href))
            {
                continue;
            }

            yield return new EmailContentListElement
            {
                Title = CleanText(title.TextContent)!,
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