namespace Bymse.Articles.Tests.Emails;

public enum BodyType
{
    PlainText,
    Html
}

public record EmailMessage(string To, string Subject, string Body, BodyType BodyType, string From);

public interface IEmailSender
{
    Task SendEmail(EmailMessage message);
}