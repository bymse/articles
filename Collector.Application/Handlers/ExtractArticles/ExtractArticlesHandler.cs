using Application.Extensions;
using Collector.Application.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.ExtractArticles;

public record ExtractArticlesCommand(Ulid ReceivedEmailId);

public class ExtractArticlesHandler(DbContext context, ExtractArticlesValidator validator)
{
    public async Task Handle(ExtractArticlesCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var email = await context.GetEntity<ReceivedEmail>(command.ReceivedEmailId, ct);
        var source = context
            .Set<ConfirmedSource>()
            .Local
            .Single(e => e.Receiver.Email == email.ToEmail);
        
        //todo extract and store articles
    }
}