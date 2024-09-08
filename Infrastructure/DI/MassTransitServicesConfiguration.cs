using System.Reflection;
using Application.Mediator;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class MassTransitServicesConfiguration
{
    private static readonly List<Assembly> Assemblies = new();

    internal static void AddConsumersAssemblies(Assembly[] assemblies)
    {
        Assemblies.AddRange(assemblies);
    }

    public static IServiceCollection AddMassTransitInfrastructure(this IServiceCollection services)
    {
        var assemblies = Assemblies.ToArray();

        services.AddMediator(x =>
        {
            x.AddConsumers(assemblies);
            x.ConfigureMediator((context, cfg) =>
            {
                cfg.UseConsumeFilter(typeof(UseCaseValidationFilter<>), context,
                    e => { e.Include(r => r.IsAssignableTo(typeof(IUseCase))); });
                cfg.UseConsumeFilter(typeof(UseCaseCommitFilter<>), context,
                    e => { e.Include(r => r.IsAssignableTo(typeof(IUseCase))); });

                cfg.UseInMemoryOutbox(context);
            });
        });

        return services;
    }
}