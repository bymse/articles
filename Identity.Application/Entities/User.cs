using Identity.Integration;

namespace Identity.Application.Entities;

public record IdPUser(string Provider, string Id); 

public class User(IdPUser idPUser)
{
    public IdentityUserId Id { get; private set; } = new(Ulid.NewUlid());
    
    public IdPUser IdPUser { get; private set; } = idPUser;
    
    public string? Email { get; set; }

    private User() : this(null!)
    {
        
    }
}