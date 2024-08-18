﻿using Application.DbContexts;
using Application.Mediator;
using Collector.Application.Entities;
using Collector.Integration;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.UseCases.Create;

public record CreateSourceUseCase(string Title, Uri WebPage) : IUseCase<UnconfirmedSourceInfo>;

public record UnconfirmedSourceInfo(CollectorSourceId Id, string Email);

public class CreateSourceHandler(IDbContextProvider<Collector> dbContextProvider)
    : UseCaseHandler<CreateSourceUseCase, UnconfirmedSourceInfo>
{
    public override async Task<UnconfirmedSourceInfo> Handle(CreateSourceUseCase useCase, CancellationToken ct)
    {
        var dbContext = dbContextProvider.Get();

        var source = new UnconfirmedSource(useCase.Title, useCase.WebPage, "");
        dbContext.Add(source);

        return new UnconfirmedSourceInfo(source.Id, source.Receiver.Email);
    }
}