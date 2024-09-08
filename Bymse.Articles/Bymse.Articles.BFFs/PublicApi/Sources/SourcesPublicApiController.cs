using Application.Mediator;
using Collector.Application.Entities;
using Collector.Application.UseCases.Create;
using Feeder.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Bymse.Articles.BFFs.PublicApi.Sources;

[Route("sources")]
public class SourcesPublicApiController(ISender sender) : PublicApiController
{
    [HttpPost]
    public async Task<UnconfirmedSourceInfo> CreateSource([FromBody] CreateSourceRequest request, CancellationToken ct)
    {
        var sourceInfo = await sender.Send(
            new CreateSourceUseCase(request.Title, request.WebPage, Tenant.User(UserId.Value)), ct
        );

        return sourceInfo;
    }
}