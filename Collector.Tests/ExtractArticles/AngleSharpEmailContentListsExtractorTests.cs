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
            new EmailContentListElement
            {
                Url = new("https://exmaple.com/3"),
                Title = "Using Windows Error Reporting in .NET",
                Description = "A look into how to use Windows Error Reporting to collect crash information for your .NET apps."
            },
            new EmailContentListElement
            {
                Url = new("https://exmaple.com/5"),
                Title = "Have you ever memory-profiled in a unit test?",
                Description = "Today's article will be a short but hopefully useful one: Ever had to memory profile code during unit test execution? Thought so. Let's take a look at how that works."
            },
            new EmailContentListElement
            {
                Url = new("https://exmaple.com/6"),
                Title = "Implementing ASP.NET Core Automatic Span Linking for Internal Redirects with Middleware",
                Description = "This article discusses implementing OpenTelemetry instrumentation for .NET applications, focusing on creating span links between request traces during internal redirects in ASP.NET Core. The author presents a middleware solution that uses TempData to preserve trace context across redirects, enabling the linking of related requests."
            },
            new EmailContentListElement
            {
                Url = new("https://exmaple.com/7"),
                Title = "Partition methods for collections in C#",
                Description = "This article will look at some partition methods for collections in C#, specifically List, ConcurrentDictionary and Dictionary."
            },
            new EmailContentListElement
            {
                Url = new("https://exmaple.com/8"),
                Title = "From .NET 6 to .NET 8, my migration experience: Entity Framework Core 8",
                Description = "An article about migration process from Entity Framework Core 6 to Entity Framework Core 8, highlighting key changes required for a successful upgrade. The author outlines three main areas of modification: updating NuGet packages, renaming certain ModelBuilder extension methods, and addressing a breaking change related to tables with triggers."
            },
        };

        result.Elements.Should().BeEquivalentTo(expected);
    }
}