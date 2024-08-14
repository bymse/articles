namespace Identity.Application.Entities;

public record UserId(Guid Value);

public class User
{
    public UserId Id { get; private set; } = new(Guid.NewGuid());
}