using System.Reflection;
using Application.Mediator;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class MassTransitDiHelper
{
    private static readonly List<Assembly> Assemblies = new();

    internal static void AddConsumersAssemblies(Assembly[] assemblies)
    {
        Assemblies.AddRange(assemblies);
    }

    public static IServiceCollection AddMassTransitInfrastructure(this IServiceCollection services)
    {
        return services
            .AddMassTransit(x =>
            {
                x.AddMediator(r =>
                {
                    r.ConfigureMediator((context, cfg) =>
                    {
                        cfg.UseSendFilter(typeof(UseCaseValidationFilter<>), context);
                        cfg.UseSendFilter(typeof(UseCaseCommitFilter<>), context);
                    });
                });
                
                x.UsingInMemory();

                foreach (var assembly in Assemblies)
                {
                    x.AddConsumers(assembly);
                }
            });
    }
}