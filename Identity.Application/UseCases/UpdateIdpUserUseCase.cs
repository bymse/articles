using Application.Mediator;
using Identity.Application.Entities;
using Identity.Infrastructure.Mediator;
using Identity.Integration;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.UseCases;

public record UpdateIdpUserUseCase(string IdpId) : IUseCase<IdentityUserId>;

public class UpdateUserUseCaseHandler(DbContext dbContext) : UseCaseHandler<UpdateIdpUserUseCase, IdentityUserId>
{
    public override async Task<IdentityUserId> Handle(UpdateIdpUserUseCase useCase, CancellationToken ct)
    {
        var user = await dbContext
            .Set<User>()
            .FirstOrDefaultAsync(e => e.IdpId.Value == useCase.IdpId, ct);

        if (user is null)
        {
            user = new User(new IdpId(useCase.IdpId));
            dbContext.Add(user);
        }

        return user.Id;
    }
}