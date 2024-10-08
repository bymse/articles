using System.Reflection;
using Application.Mediator;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure.ServicesConfiguration;

public static class UseCasesServicesConfiguration
{
    public static IServiceCollection AddUseCases(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.TryAddScoped<ISender, Sender>();
        MassTransitServicesConfiguration.AddMediatorAssemblies(assemblies);

        foreach (var assembly in assemblies)
        {
            var scanner = AssemblyScanner.FindValidatorsInAssembly(assembly);
            foreach (var result in scanner)
            {
                services.TryAddScoped(result.InterfaceType, result.ValidatorType);
            }
        }

        return services;
    }
}