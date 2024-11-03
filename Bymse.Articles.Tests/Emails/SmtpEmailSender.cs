using System.Net.Mail;

namespace Bymse.Articles.Tests.Emails;

public class SmtpEmailSender : IEmailSender
{
    public async Task SendEmail(EmailMessage message)
    {
        using var smtpClient = new SmtpClient("localhost", 13025);

        var mailMessage = new MailMessage
        {
            From = new MailAddress(message.From),
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = message.BodyType == BodyType.Html
        };

        mailMessage.To.Add(message.To);

        await smtpClient.SendMailAsync(mailMessage);
    }
}