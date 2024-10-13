using Collector.Application.Services;
using Collector.Infrastructure.Html;
using FluentAssertions;

namespace Collector.Tests.ExtractArticles;

public class AngleSharpEmailContentListsExtractorTests
{
    private readonly AngleSharpEmailContentListsExtractor extractor = new();

    [TestCase("")]
    [TestCase("some string")]
    public async Task Should_ReturnEmptyList_OnNonHtml(string html)
    {
        var result = await extractor.ExtractFromHtml(html).ToArrayAsync();
        result.Should().BeEmpty();
    }

    [Test]
    public async Task Should_ReturnArticlesLists_ForCSharpDigest()
    {
        var html = await File.ReadAllTextAsync("TestData/csharp-digest.html");
        var articlesList = new EmailContentList
        {
            Elements =
            [
                new EmailContentListElement { Url = new Uri("https://example.com/article1"), Title = "Article 1" },
                new EmailContentListElement { Url = new Uri("https://example.com/article2"), Title = "Article 2" },
                new EmailContentListElement { Url = new Uri("https://example.com/article3"), Title = "Article 3" }
            ]
        };

        var reactionList = new EmailContentList
        {
            Elements =
            [
                new EmailContentListElement { Url = new Uri("https://example.com/reaction1"), Title = "Reaction 1" },
                new EmailContentListElement { Url = new Uri("https://example.com/reaction2"), Title = "Reaction 2" },
                new EmailContentListElement { Url = new Uri("https://example.com/reaction3"), Title = "Reaction 3" }
            ]
        };

        var newsLettersList = new EmailContentList
        {
            Elements =
            [
                new EmailContentListElement
                    { Url = new Uri("https://example.com/newsletter1"), Title = "Newsletter 1" },
                new EmailContentListElement
                    { Url = new Uri("https://example.com/newsletter2"), Title = "Newsletter 2" },
                new EmailContentListElement { Url = new Uri("https://example.com/newsletter3"), Title = "Newsletter 3" }
            ]
        };

        var adsNoticeList = new EmailContentList
        {
            Elements =
            [
                new EmailContentListElement { Url = new Uri("https://example.com/ads1"), Title = "Ads 1" },
                new EmailContentListElement { Url = new Uri("https://example.com/ads2"), Title = "Ads 2" }
            ]
        };

        var subscriptionSettings = new EmailContentList
        {
            Elements =
            [
                new EmailContentListElement { Url = new Uri("https://example.com/settings1"), Title = "Settings 1" },
                new EmailContentListElement { Url = new Uri("https://example.com/settings2"), Title = "Settings 2" }
            ]
        };

        var result = await extractor.ExtractFromHtml(html).ToArrayAsync();
        
        result.Should()
            .BeEquivalentTo([
                articlesList, reactionList, newsLettersList, adsNoticeList, subscriptionSettings
            ], e => e.WithStrictOrdering());
    }
}