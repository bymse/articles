using Application.Mediator;
using Collector.Application.Entities;
using Collector.Integration;

namespace Collector.Application.UseCases.Create;

public record CreateSourceUseCase(string Title, Uri WebPage, Tenant Tenant)
    : IUseCase<UnconfirmedSourceInfo>;

public record UnconfirmedSourceInfo(CollectorSourceId Id, string Email);