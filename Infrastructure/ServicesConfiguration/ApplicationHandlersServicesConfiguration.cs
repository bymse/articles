using System.Reflection;
using Application.Handlers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure.ServicesConfiguration;

public static class ApplicationHandlersServicesConfiguration
{
    public static IServiceCollection AddApplicationHandlers(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var scanner = AssemblyScanner.FindValidatorsInAssembly(assembly);
            foreach (var result in scanner)
            {
                services.TryAddScoped(result.ValidatorType);
            }

            foreach (var handlerType in assembly.GetTypes()
                         .Where(e => e.IsAssignableTo(typeof(IApplicationHandler))))
            {
                services.TryAddScoped(handlerType);
            }
        }

        return services;
    }
}