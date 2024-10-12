using Collector.Application.Entities;
using Collector.Application.Handlers.CreateSource;
using Microsoft.AspNetCore.Mvc;

namespace Bymse.Articles.BFFs.PublicApi.Sources;

public class SourcesController : PublicApiController
{
    [HttpPost]
    public async Task<UnconfirmedSourceInfo> CreateSource(
        [FromBody] CreateSourceRequest request,
        [FromServices] CreateSourceHandler handler,
        CancellationToken ct)
    {
        var sourceInfo = await handler.Handle(
            new CreateSourceCommand(request.Title, request.WebPage, Tenant.User(UserId.Value)), ct
        );

        return sourceInfo;
    }
}