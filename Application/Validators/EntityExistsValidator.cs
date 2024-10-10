using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Validators;

public class EntityExistsValidator<TEntity> : AbstractValidator<Ulid> where TEntity : class
{
    public EntityExistsValidator(DbContext context)
    {
        RuleFor(e => e)
            .MustAsync(async (ulid, ct) => await context.FindAsync<TEntity>([ulid], ct) != null);
    }
}