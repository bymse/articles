namespace Identity.Application.Entities;

public record IdPUser
{
    public const int MaxLength = 100;

    public IdPUser(string Provider, string Id)
    {
        if (Provider.Length > MaxLength)
        {
            throw new ArgumentException($"Provider must be {MaxLength} characters or less", nameof(Provider));
        }

        if (Id.Length > MaxLength)
        {
            throw new ArgumentException($"Id must be {MaxLength} characters or less", nameof(Id));
        }

        this.Provider = Provider;
        this.Id = Id;
    }

    public string Provider { get; }
    public string Id { get; }
}