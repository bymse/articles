using Collector.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Handlers.GetSources;

public record GetSourcesQuery(Tenant Tenant);

public class GetSourcesHandler(DbContext context)
{
    public async Task<SourceInfoCollection> Handle(GetSourcesQuery query, CancellationToken ct)
    {
        var sources = await context.Set<Source>()
            .Where(s => s.Tenant == query.Tenant)
            .ToListAsync(ct);

        return new SourceInfoCollection
        {
            Items = sources.Select(s => new SourceInfo
            {
                Id = s.Id.Value,
                Title = s.Title,
                WebPage = s.WebPage,
                State = s.State,
                ReceiverEmail = s.Receiver.Email
            }).ToArray()
        };
    }
}