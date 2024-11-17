using System.Net.Mail;

namespace Bymse.Articles.Tests.Emails;

public class SmtpEmailSender(string host = "localhost", int port = 13025) : IEmailSender
{
    public async Task SendEmail(EmailMessage message)
    {
        using var smtpClient = new SmtpClient(host, port);

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