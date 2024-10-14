using Application.Extensions;
using Collector.Application.Entities;
using Collector.Application.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.ExtractArticles;

public record SaveArticlesCommand(Ulid ReceivedEmailId);

public class SaveArticlesHandler(DbContext context, SaveArticlesValidator validator, EmailArticlesExtractor extractor)
{
    public async Task Handle(SaveArticlesCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var email = await context.GetEntity<ReceivedEmail>(command.ReceivedEmailId, ct);
        var source = context
            .Set<ConfirmedSource>()
            .Local
            .Single(e => e.Receiver.Email == email.ToEmail);

        await foreach (var emailArticleInfo in extractor.Extract(email, ct))
        {
            
        }
    }
}