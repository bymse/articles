using Application.Mediator;
using Collector.Application.Entities;
using Collector.Application.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Collector.Application.UseCases.Create;

public class CreateSourceHandler(DbContext dbContext, IOptions<CollectorApplicationSettings> settings)
    : UseCaseHandler<CreateSourceUseCase, UnconfirmedSourceInfo>
{
    protected override async Task<UnconfirmedSourceInfo> Handle(CreateSourceUseCase useCase, CancellationToken ct)
    {
        var source = new UnconfirmedSource(
            useCase.Title,
            useCase.WebPage,
            settings.Value.Domain,
            useCase.Tenant
        );
        dbContext.Add(source);

        return new UnconfirmedSourceInfo(source.Id, source.Receiver.Email);
    }
}