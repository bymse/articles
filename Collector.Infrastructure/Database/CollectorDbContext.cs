﻿using Collector.Application;
using Collector.Application.Entities;
using Collector.Integration;
using Infrastructure.ServicesConfiguration;
using Infrastructure.Ulids;
using Microsoft.EntityFrameworkCore;

namespace Collector.Infrastructure.Database;

public class CollectorDbContext(DbContextOptions<CollectorDbContext> options) : DbContext(options), IKeyedDbContext
{
    public static string Key => CollectorConstants.Key;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<CollectorSourceId>()
            .HaveConversion<CollectorSourceIdConverter>();

        configurationBuilder.ComplexProperties<Receiver>();

        configurationBuilder
            .Properties<Ulid>()
            .HaveConversion<UlidValueConverter>();
    }
}