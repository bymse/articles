using Collector.Application.Entities;
using FluentResults;

namespace Collector.Application.Services;

public class EmailSubscriptionConfirmationService(IHtmlLinksFinder htmlLinksFinder)
{
    public async Task<IResultBase> TryConfirm(ReceivedEmail email)
    {
        var link = await FindConfirmationLink(email);
        if (link is null)
        {
            return Result.Fail("No confirmation link found in email");
        }
        
        
        return Result.Ok();
    }

    private async Task<Uri?> FindConfirmationLink(ReceivedEmail email)
    {
        if (string.IsNullOrWhiteSpace(email.HtmlBody))
        {
            return null;
        }

        var links = await htmlLinksFinder.FindLinks(email.HtmlBody);

        return links
            .FirstOrDefault(link => link.Text.Contains("confirm", StringComparison.OrdinalIgnoreCase))
            ?.Url;
    }
}