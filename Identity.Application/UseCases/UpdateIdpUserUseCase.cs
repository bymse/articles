using Application.Mediator;
using Identity.Application.Entities;
using Identity.Infrastructure.Mediator;
using Identity.Integration;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.UseCases;

public record UpdateIdpUserUseCase(string IdpName, string IdpUserId) : IUseCase<IdentityUserId>;

public class UpdateUserUseCaseHandler(DbContext dbContext) : UseCaseHandler<UpdateIdpUserUseCase, IdentityUserId>
{
    public override async Task<IdentityUserId> Handle(UpdateIdpUserUseCase useCase, CancellationToken ct)
    {
        var user = await dbContext
            .Set<User>()
            .Where(e => e.IdP.Name == useCase.IdpName)
            .FirstOrDefaultAsync(e => e.IdP.UserId == useCase.IdpUserId, ct);

        if (user is null)
        {
            user = new User(new IdP(useCase.IdpName, useCase.IdpUserId));
            dbContext.Add(user);
        }

        return user.Id;
    }
}