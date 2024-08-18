using Identity.Integration;
using JetBrains.Annotations;

namespace Identity.Application.Entities;

public class User(IdPUser idPUser)
{
    public const int MaxEmailLength = 100;

    public IdentityUserId Id { get; private set; } = new(Ulid.NewUlid());

    public IdPUser IdPUser { get; private set; } = idPUser;

    public string? Email { get; private set; }

    public void SetEmail(string email)
    {
        if (email.Length > MaxEmailLength)
        {
            throw new ArgumentException("Email must be 100 characters or less", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email must not be empty", nameof(email));
        }

        if (!email.Contains('@'))
        {
            throw new ArgumentException("Email must contain @", nameof(email));
        }

        Email = email;
    }
    
    [UsedImplicitly]
    private User() : this(null!)
    {
    }
}