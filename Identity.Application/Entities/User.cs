namespace Identity.Application.Entities;

public record UserId(Guid Value);

public record IdpId(string Value); 

public class User(IdpId idpId)
{
    public UserId Id { get; private set; } = new(Guid.NewGuid());
    
    public IdpId IdpId { get; private set; } = idpId;
}