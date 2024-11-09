using System.Runtime.CompilerServices;
using Application.Di;
using Collector.Application.Entities;

namespace Collector.Application.Services;

public record EmailArticleInfo(Uri Url, string Title, string? Description);

[AutoRegistration]
public class EmailArticlesExtractor(IEmailContentListsExtractor emailContentListsExtractor)
{
    private static readonly ISet<string> TitleStopList = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "here",
        "media kit",
        "Leadership in Tech",
        "Programming Digest",
        "C# Digest",
        "React Digest",
        "\ud83d\udc4e Bad",
        "\ud83d\udc4c Amazing",
        "\ud83d\ude42 Good",
        "Read Online"
    };

    public async IAsyncEnumerable<EmailArticleInfo> Extract(ReceivedEmail email, ConfirmedSource source,
        [EnumeratorCancellation] CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(email.HtmlBody))
        {
            yield break;
        }

        await foreach (var list in emailContentListsExtractor
                           .ExtractFromHtml(email.HtmlBody, source.Type)
                           .WithCancellation(ct))
        {
            foreach (var element in list.Elements)
            {
                if (TitleStopList.Contains(element.Title))
                {
                    continue;
                }

                yield return new EmailArticleInfo(element.Url, element.Title, element.Description);
            }
        }
    }
}