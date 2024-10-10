using Collector.Application.Entities;

namespace Collector.Application.Services;

public class EmailClassifier
{
    public Task<EmailType> Classify(EmailModel email)
    {
        if (IsArticlesEmail(email))
        {
            return Task.FromResult(EmailType.Articles);
        }
        
        if (IsConfirmationEmail(email))
        {
            return Task.FromResult(EmailType.Confirmation);
        }
        
        return Task.FromResult(EmailType.Unknown);
    }
    
    private static bool IsArticlesEmail(EmailModel info)
    {
        if (info.Headers.ContainsKey("List-Unsubscribe"))
        {
            return true;
        }

        return false;
    }

    private static bool IsConfirmationEmail(EmailModel model)
    {
        if (model.Subject.Contains("confirm", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (model.HtmlBody?.Contains("confirm", StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        if (model.TextBody?.Contains("confirm", StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        return false;
    }
}