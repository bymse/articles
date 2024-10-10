namespace Collector.Application.Entities;

public class ManualProcessingEmail
{
    public Ulid Id { get; private init; } = Ulid.NewUlid();

    public Ulid ReceivedEmailId { get; init; }

    public string? Description { get; init; }

    public ManualProcessingEmailType Type { get; init; }

    public static ManualProcessingEmail UnknownEmail(Ulid receivedEmailId)
    {
        return new ManualProcessingEmail
        {
            ReceivedEmailId = receivedEmailId,
            Type = ManualProcessingEmailType.UnknownEmailType,
        };
    }

    public static ManualProcessingEmail FailedToConfirm(Ulid receivedEmailId, string message)
    {
        return new ManualProcessingEmail
        {
            ReceivedEmailId = receivedEmailId,
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