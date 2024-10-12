using Collector.Application.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.ConfirmSource;

public class ConfirmSourceValidator : AbstractValidator<ConfirmSourceCommand>
{
    public ConfirmSourceValidator(DbContext context)
    {
        RuleFor(c => c.ReceiverEmail)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, ct) =>
            {
                var source = await context
                    .Set<UnconfirmedSource>()
                    .SingleOrDefaultAsync(s => s.Receiver.Email == email, ct);

                return source != null;
            });
    }
}