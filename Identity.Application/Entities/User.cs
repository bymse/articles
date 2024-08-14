using Identity.Integration;

namespace Identity.Application.Entities;

public record IdpId(string Value); 

public class User(IdpId idpId)
{
    public IdentityUserId Id { get; private set; } = new(Guid.NewGuid());
    
    public IdpId IdpId { get; private set; } = idpId;
}