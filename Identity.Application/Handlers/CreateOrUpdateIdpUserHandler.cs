using Identity.Application.Entities;
using Identity.Integration;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Handlers;

public record CreateOrUpdateIdpUserCommand(string IdpName, string IdpUserId, string Email);

public class CreateOrUpdateIdpUserHandler(DbContext dbContext)
{
    public async Task<IdentityUserId> Handle(CreateOrUpdateIdpUserCommand command, CancellationToken ct)
    {
        var user = await dbContext
            .Set<User>()
            .Where(e => e.IdPUser.Provider == command.IdpName)
            .FirstOrDefaultAsync(e => e.IdPUser.Id == command.IdpUserId, ct);

        if (user is null)
        {
            user = new User(new IdPUser(command.IdpName, command.IdpUserId));
            dbContext.Add(user);
        }

        user.SetEmail(command.Email);

        await dbContext.SaveChangesAsync(ct);
        return user.Id;
    }
}