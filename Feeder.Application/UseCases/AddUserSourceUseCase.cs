using Application.Mediator;
using Collector.Integration;
using Feeder.Application.Entities;
using FluentValidation;
using Identity.Integration;
using Microsoft.EntityFrameworkCore;

namespace Feeder.Application.UseCases;

public record AddUserSourceUseCase(IdentityUserId UserId, CollectorSourceId SourceId) : IUseCase;

public class AddUserSourceUseCaseValidator : AbstractValidator<AddUserSourceUseCase>
{
    public AddUserSourceUseCaseValidator(DbContext dbContext)
    {
        RuleFor(e => e)
            .MustAsync(async (uc, ct) =>
            {
                return !await dbContext
                    .Set<UserSource>()
                    .Where(e => e.UserId == uc.UserId)
                    .AnyAsync(e => e.SourceId == uc.SourceId, ct);
            })
            .WithErrorCode("UserSource.AlreadyExists");
    }
}

public class AddUserSourceUseCaseHandler(DbContext dbContext) : UseCaseHandler<AddUserSourceUseCase>
{
    protected async override Task Handle(AddUserSourceUseCase useCase, CancellationToken ct)
    {
        var userSource = new UserSource(useCase.UserId, useCase.SourceId);

        dbContext.Add(userSource);
    }
}