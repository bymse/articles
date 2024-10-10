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

    public static ManualProcessingEmail Confirm(ReceivedEmail email)
    {
        return new ManualProcessingEmail
        {
            ReceivedEmailId = email.Id,
            Type = ManualProcessingEmailType.ConfirmSubscription,
        };
    }
}

public enum ManualProcessingEmailType
{
    UnknownEmailType,
    ConfirmSubscription,
}