using Application.DbContexts;
using Application.Mediator;
using Collector.Application.Entities;
using Collector.Application.Settings;
using Collector.Integration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Collector.Application.UseCases.Create;

public class CreateSourceHandler(IUseCaseDbContextProvider provider, IOptions<CollectorApplicationSettings> settings)
    : UseCaseHandler<CreateSourceUseCase, UnconfirmedSourceInfo>
{
    protected override async Task<UnconfirmedSourceInfo> Handle(CreateSourceUseCase useCase, CancellationToken ct)
    {
        var dbContext = provider.GetFor<CreateSourceUseCase>();

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