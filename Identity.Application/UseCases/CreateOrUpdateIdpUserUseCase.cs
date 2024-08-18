﻿using Application.Mediator;
using Identity.Application.Entities;
using Identity.Infrastructure.Mediator;
using Identity.Integration;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.UseCases;

public record CreateOrUpdateIdpUserUseCase(string IdpName, string IdpUserId, string Email) : IUseCase<IdentityUserId>;

public class CreateOrUpdateIdpUserHandler(DbContext dbContext) : UseCaseHandler<CreateOrUpdateIdpUserUseCase, IdentityUserId>
{
    public override async Task<IdentityUserId> Handle(CreateOrUpdateIdpUserUseCase useCase, CancellationToken ct)
    {
        var user = await dbContext
            .Set<User>()
            .Where(e => e.IdP.Provider == useCase.IdpName)
            .FirstOrDefaultAsync(e => e.IdP.UserId == useCase.IdpUserId, ct);

        if (user is null)
        {
            user = new User(new IdP(useCase.IdpName, useCase.IdpUserId));
            dbContext.Add(user);
        }

        user.Email = useCase.Email;

        return user.Id;
    }
}