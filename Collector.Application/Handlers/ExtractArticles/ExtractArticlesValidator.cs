using Application.Extensions;
using Collector.Application.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.ExtractArticles;

public class ExtractArticlesValidator : AbstractValidator<ExtractArticlesCommand>
{
    public ExtractArticlesValidator(DbContext context)
    {
        RuleFor(x => x.ReceivedEmailId)
            .MustAsync(async (id, ct) =>
            {
                var email = await context.FindEntity<ReceivedEmail>(id, ct);
                return email?.Type == EmailType.Articles;
            })
            .WithErrorCode("Email.WrongType")
            .MustAsync(async (id, ct) =>
            {
                var email = await context.FindEntity<ReceivedEmail>(id, ct);
                if (email == null)
                {
                    return true;
                }

                var source = await context
                    .Set<ConfirmedSource>()
                    .FirstOrDefaultAsync(e => e.Receiver.Email == email.ToEmail, ct);

                return source != null;
            })
            .WithErrorCode("Source.NotFoundConfirmed");
    }
}