namespace Bymse.Articles.Tests.Emails;

public record EmailMessage(string To, string Subject, string Body, string BodyType, string From);

public interface IEmailSender
{
    Task SendEmail(EmailMessage message);
}