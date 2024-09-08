using Application.DbContexts;
using Application.Mediator;
using Collector.Application.Entities;
using Collector.Integration;
using Microsoft.Extensions.Configuration;

namespace Collector.Application.UseCases.Create;

public class CreateSourceHandler(IUseCaseDbContextProvider provider, IConfiguration configuration)
    : UseCaseHandler<CreateSourceUseCase, UnconfirmedSourceInfo>
{
    protected override async Task<UnconfirmedSourceInfo> Handle(CreateSourceUseCase useCase, CancellationToken ct)
    {
        var dbContext = provider.GetFor<CreateSourceUseCase>();

        var source = new UnconfirmedSource(
            useCase.Title,
            useCase.WebPage,
            "",
            useCase.Tenant
        );
        dbContext.Add(source);

        return new UnconfirmedSourceInfo(source.Id, source.Receiver.Email);
    }
}