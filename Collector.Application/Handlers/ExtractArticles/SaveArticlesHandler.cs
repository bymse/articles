using Application.Events;
using Application.Extensions;
using Collector.Application.Entities;
using Collector.Application.Services;
using Collector.Integration;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Registry.Integration;

namespace Collector.Application.Handlers.ExtractArticles;

public record SaveArticlesCommand(Ulid ReceivedEmailId);

public class SaveArticlesHandler(
    DbContext context,
    SaveArticlesValidator validator,
    EmailArticlesExtractor extractor,
    IPublisher publisher)
{
    public async Task Handle(SaveArticlesCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);

        var email = await context.GetEntity<ReceivedEmail>(command.ReceivedEmailId, ct);
        var source = context
            .Set<ConfirmedSource>()
            .Local
            .Single(e => e.Receiver.Email == email.ToEmail);

        await foreach (var emailArticleInfo in extractor.Extract(email, source, ct))
        {
            var task = new SaveArticleTask
            {
                Url = emailArticleInfo.Url,
                Title = emailArticleInfo.Title,
                Tags =
                [
                    new ArticleTag(CollectorWellKnownTags.SourceIds, source.Id.ToString()),
                    new ArticleTag(CollectorWellKnownTags.From, email.FromName ?? email.FromEmail),
                    new ArticleTag(CollectorWellKnownTags.EmailIds, email.Id.ToString())
                ],
                Desription = Truncate(emailArticleInfo.Description, 300)
            };

            await publisher.PublishTask(task, ct);
            await context.SaveChangesAsync(ct);
        }
    }
    
    private static string? Truncate(string? value, int maxLength)
    {
        if (value == null)
        {
            return null;
        }

        return value.Length > maxLength
            ? value.Substring(0, maxLength)
            : value;
    }
}