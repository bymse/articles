﻿using Collector.Infrastructure.Database;
using Infrastructure.ServicesConfiguration;
using Microsoft.Extensions.DependencyInjection;

namespace Collector.Infrastructure;

public static class CollectorModule
{
    public static IServiceCollection AddCollectorServices(this IServiceCollection services)
    {
        return services
            .AddPostgresDbContext<CollectorDbContext>()
            .AddUseCases(typeof(Application.CollectorConstants).Assembly);
    }
}