using Application.Extensions;
using Collector.Application.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.ConfirmSource;

public class ConfirmSourceValidator : AbstractValidator<ConfirmSourceCommand>
{
    public ConfirmSourceValidator(DbContext context)
    {
        RuleFor(c => c.ReceivedEmailId)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (id, ct) =>
            {
                var email = await context.FindEntity<ReceivedEmail>(id, ct);
                return email != null;
            })
            .WithErrorCode("Email.NotFound")
            .MustAsync(async (id, ct) =>
            {
                var email = await context.GetEntity<ReceivedEmail>(id, ct);
                var source = await context
                    .Set<UnconfirmedSource>()
                    .SingleOrDefaultAsync(s => s.Receiver.Email == email.ToEmail, ct);
                
                return source != null;
            })
            .WithErrorCode("Source.NotFoundUnconfirmed");
    }
}