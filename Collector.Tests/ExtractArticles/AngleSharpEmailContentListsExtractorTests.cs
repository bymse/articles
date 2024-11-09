using Collector.Application.Entities;
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
        var result = await extractor.ExtractFromHtml(html, SourceType.BonoboEmailDigest, default);
        result.Elements.Should().BeEmpty();
    }

    [Test]
    public async Task Should_ReturnArticlesLists_ForCSharpDigest()
    {
        var html = await File.ReadAllTextAsync("TestData/BonoboEmailDigest.html");

        var result = await extractor.ExtractFromHtml(html, SourceType.BonoboEmailDigest, default);

        var expected = new[]
        {
            new EmailContentListElement()
        };

        result.Elements.Should().BeEquivalentTo(expected);
    }
}