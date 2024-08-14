using Application.Mediator;
using Identity.Application.Entities;
using Identity.Infrastructure.Mediator;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.UseCases;

public record UpdateIdpUserUseCase(string IdpId) : IUseCase<UserId>;

public class UpdateUserUseCaseHandler(DbContext dbContext) : UseCaseHandler<UpdateIdpUserUseCase, UserId>
{
    public override async Task<UserId> Handle(UpdateIdpUserUseCase useCase, CancellationToken ct)
    {
        var user = await dbContext
            .Set<User>()
            .FirstOrDefaultAsync(e => e.IdpId.Value == useCase.IdpId, ct);

        if (user is null)
        {
            user = new User(new IdpId(useCase.IdpId));
            dbContext.Add(user);
        }
        
        await dbContext.SaveChangesAsync(ct);
    }
}