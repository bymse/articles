using Collector.Application.Entities;
using Collector.Application.Handlers.ConfirmSource;
using Collector.Application.Handlers.CreateSource;
using Microsoft.AspNetCore.Mvc;

namespace Bymse.Articles.BFFs.Public.Sources;

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

    [HttpPost("confirm")]
    public async Task ConfirmSource(
        [FromBody] ConfirmSourceRequest request,
        [FromServices] ConfirmSourceHandler handler,
        CancellationToken ct)
    {
        await handler.Handle(new ConfirmSourceCommand(request.ReceiverEmail), ct);
    }
}