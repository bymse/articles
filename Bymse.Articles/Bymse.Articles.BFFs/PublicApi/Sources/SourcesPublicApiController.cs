using Application.Mediator;
using Collector.Application.UseCases.Create;
using Feeder.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Bymse.Articles.BFFs.PublicApi.Sources;

[Route("sources")]
public class SourcesPublicApiController(IUseCaseExecutor useCaseExecutor) : PublicApiController
{
    [HttpPost]
    public async Task<UnconfirmedSourceInfo> CreateSource([FromBody] CreateSourceRequest request, CancellationToken ct)
    {
        var sourceInfo = await useCaseExecutor.Execute(new CreateSourceUseCase(request.Title, request.WebPage), ct);
        
        await useCaseExecutor.Execute(new AddUserSourceUseCase(UserId, sourceInfo.Id), ct);
        
        return sourceInfo;
    }
}