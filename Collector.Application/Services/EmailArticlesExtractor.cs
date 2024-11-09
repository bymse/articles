using System.Runtime.CompilerServices;
using Application.Di;
using Collector.Application.Entities;

namespace Collector.Application.Services;

public record EmailArticleInfo(Uri Url, string Title, string? Description);

[AutoRegistration]
public class EmailArticlesExtractor(IEmailContentListsExtractor emailContentListsExtractor)
{
    public async IAsyncEnumerable<EmailArticleInfo> Extract(ReceivedEmail email, ConfirmedSource source,
        [EnumeratorCancellation] CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(email.HtmlBody))
        {
            yield break;
        }

        var model = await emailContentListsExtractor.ExtractFromHtml(email.HtmlBody, source.Type, ct);

        foreach (var element in model.Elements)
        {
            yield return new EmailArticleInfo(element.Url, element.Title, element.Description);
        }
    }
}