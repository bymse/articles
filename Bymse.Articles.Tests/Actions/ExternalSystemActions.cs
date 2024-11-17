using Bymse.Articles.PublicApi.Client;
using Bymse.Articles.Tests.Emails;

namespace Bymse.Articles.Tests.Actions;

public class ExternalSystemActions(IEmailSender emailSender) : IExternalSystemActions
{
    public async Task<EmailMessage> SendConfirmationEmail(UnconfirmedSourceInfo source)
    {
        var message = new EmailMessage(
            source.Email,
            "Test subject",
            "Test body",
            BodyType.PlainText,
            "me@localhost"
        );
        await emailSender.SendEmail(message);
        await Task.Delay(TimeSpan.FromSeconds(15));

        return message;
    }

    public async Task<EmailMessage> SendArticlesEmail(SourceInfo source, string fileName)
    {
        var message = new EmailMessage(
            source.ReceiverEmail,
            "Articles subject",
            await File.ReadAllTextAsync($"TestData/{fileName}"),
            BodyType.Html,
            "articles@localhost"
        );
        
        await emailSender.SendEmail(message);
        await Task.Delay(TimeSpan.FromSeconds(15));
        
        return message;
    }
}