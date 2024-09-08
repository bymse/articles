using Application.DbContexts;
using Application.Mediator;
using Collector.Application.Entities;
using Collector.Integration;
using MassTransit.Mediator;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.UseCases.Create;

public record CreateSourceUseCase(string Title, Uri WebPage, Tenant Tenant)
    : IUseCase<UnconfirmedSourceInfo>;

public record UnconfirmedSourceInfo(CollectorSourceId Id, string Email);

public class CreateSourceHandler(IUseCaseDbContextProvider provider)
    : MediatorRequestHandler<CreateSourceUseCase, UnconfirmedSourceInfo>
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

        return new UnconfirmedSourceInfo(new CollectorSourceId(Ulid.Empty), useCase.Title);
    }
}