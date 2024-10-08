using Application.Mediator;
using Identity.Application.Entities;
using Identity.Integration;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.UseCases;

public record CreateOrUpdateIdpUserUseCase(string IdpName, string IdpUserId, string Email) : IUseCase<IdentityUserId>;

public class CreateOrUpdateIdpUserHandler(DbContext dbContext)
    : UseCaseHandler<CreateOrUpdateIdpUserUseCase, IdentityUserId>
{
    protected override async Task<IdentityUserId> Handle(CreateOrUpdateIdpUserUseCase useCase, CancellationToken ct)
    {
        var user = await dbContext
            .Set<User>()
            .Where(e => e.IdPUser.Provider == useCase.IdpName)
            .FirstOrDefaultAsync(e => e.IdPUser.Id == useCase.IdpUserId, ct);

        if (user is null)
        {
            user = new User(new IdPUser(useCase.IdpName, useCase.IdpUserId));
            dbContext.Add(user);
        }

        user.SetEmail(useCase.Email);

        return user.Id;
    }
}