using Application.Handlers;
using Collector.Application.Entities;
using Collector.Application.Settings;
using Collector.Integration;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Collector.Application.Handlers.Create;

public record CreateSourceCommand(string Title, Uri WebPage, Tenant Tenant);

public record UnconfirmedSourceInfo(CollectorSourceId Id, string Email);

public class CreateSourceHandler(
    DbContext dbContext,
    CreateSourceValidator validator,
    IOptions<CollectorApplicationSettings> settings) : IApplicationHandler
{
    public async Task<UnconfirmedSourceInfo> Handle(CreateSourceCommand command, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(command, ct);
        
        var source = new UnconfirmedSource(
            command.Title,
            command.WebPage,
            settings.Value.Domain,
            command.Tenant
        );
        dbContext.Add(source);
        await dbContext.SaveChangesAsync(ct);

        return new UnconfirmedSourceInfo(source.Id, source.Receiver.Email);
    }
}