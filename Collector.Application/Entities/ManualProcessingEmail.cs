namespace Collector.Application.Entities;

public class ManualProcessingEmail
{
    public Ulid Id { get; private init; } = Ulid.NewUlid();

    public Ulid ReceivedEmailId { get; init; }

    public string? Description { get; init; }

    public ManualProcessingEmailType Type { get; init; }

    public static ManualProcessingEmail UnknownEmail(ReceivedEmail email)
    {
        return new ManualProcessingEmail
        {
            ReceivedEmailId = email.Id,
            Type = ManualProcessingEmailType.UnknownEmailType,
        };
    }

    public static ManualProcessingEmail FailedToConfirm(ReceivedEmail email, string message)
    {
        return new ManualProcessingEmail
        {
            ReceivedEmailId = email.Id,
            Type = ManualProcessingEmailType.FailedToConfirmSubscription,
            Description = message
        };
    }
}

public enum ManualProcessingEmailType
{
    UnknownEmailType,
    FailedToConfirmSubscription,
}