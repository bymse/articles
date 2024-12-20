﻿using Bymse.Articles.Apis.Public.Sources.Models;
using Collector.Application.Entities;
using Collector.Application.Handlers.ConfirmSource;
using Collector.Application.Handlers.CreateSource;
using Collector.Application.Handlers.GetSources;
using Microsoft.AspNetCore.Mvc;

namespace Bymse.Articles.Apis.Public.Sources;

public class SourcesController : PublicApiController
{
    [HttpPost]
    public async Task<UnconfirmedSourceInfo> CreateSource(
        [FromBody] CreateSourceRequest request,
        [FromServices] CreateSourceHandler handler,
        CancellationToken ct)
    {
        var sourceInfo = await handler.Handle(
            new CreateSourceCommand(request.Title, request.WebPage, Tenant.User(UserId.Value), request.Type), ct
        );

        return sourceInfo;
    }

    [HttpPost("confirm")]
    public async Task ConfirmSource(
        [FromBody] ConfirmSourceRequest request,
        [FromServices] ConfirmSourceHandler handler,
        CancellationToken ct)
    {
        await handler.Handle(new ConfirmSourceCommand(request.ReceivedEmailId), ct);
    }

    [HttpGet]
    public async Task<SourceInfoCollection> GetSources(
        [FromServices] GetSourcesHandler handler,
        CancellationToken ct
    )
    {
        return await handler.Handle(new GetSourcesQuery(Tenant.User(UserId.Value)), ct);
    }
}