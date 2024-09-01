using Application.DbContexts;
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
    public AddUserSourceUseCaseValidator(IUseCaseDbContextProvider useCaseDbContextProvider)
    {
        RuleFor(e => e)
            .MustAsync(async (uc, ct) =>
            {
                var dbContext = useCaseDbContextProvider.GetFor<AddUserSourceUseCase>();
                return !await dbContext
                    .Set<UserSource>()
                    .Where(e => e.UserId == uc.UserId)
                    .AnyAsync(e => e.SourceId == uc.SourceId, ct);
            })
            .WithErrorCode("UserSource.AlreadyExists");
    }
}

public class AddUserSourceUseCaseHandler(IUseCaseDbContextProvider useCaseDbContextProvider)
    : UseCaseHandler<AddUserSourceUseCase>
{
    public async override Task Handle(AddUserSourceUseCase useCase, CancellationToken ct)
    {
        var dbContext = useCaseDbContextProvider.GetFor<AddUserSourceUseCase>();
        var userSource = new UserSource(useCase.UserId, useCase.SourceId);

        dbContext.Add(userSource);
    }
}