namespace Bymse.Articles.Apis.Public.Sources.Models;

public class ConfirmSourceRequest
{
    public Ulid ReceivedEmailId { get; init; } = default!;
}