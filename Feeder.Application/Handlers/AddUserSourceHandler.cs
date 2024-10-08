using Application.Handlers;
using Collector.Integration;
using Feeder.Application.Entities;
using FluentValidation;
using Identity.Integration;
using Microsoft.EntityFrameworkCore;

namespace Feeder.Application.Handlers;

public record AddUserSourceCommand(IdentityUserId UserId, CollectorSourceId SourceId);

public class AddUserSourceValidator : AbstractValidator<AddUserSourceCommand>
{
    public AddUserSourceValidator(DbContext dbContext)
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

public class AddUserSourceHandler(AddUserSourceValidator validator, DbContext dbContext) : IApplicationHandler
{
    public async Task Handle(AddUserSourceCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var userSource = new UserSource(command.UserId, command.SourceId);

        dbContext.Add(userSource);
        await dbContext.SaveChangesAsync(ct);
    }
}