using Application.Mediator;
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
        var sourceInfo = await sender.SendRequest(new CreateSourceUseCase(request.Title, request.WebPage), ct);

        await sender.Send(new AddUserSourceUseCase(UserId, sourceInfo.Id), ct);

        return sourceInfo;
    }
}