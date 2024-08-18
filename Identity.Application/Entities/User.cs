using Identity.Integration;

namespace Identity.Application.Entities;

public record IdP(string Provider, string UserId); 

public class User(IdP idP)
{
    public IdentityUserId Id { get; private set; } = new(Guid.NewGuid());
    
    public IdP IdP { get; private set; } = idP;
    
    public string? Email { get; set; }
}