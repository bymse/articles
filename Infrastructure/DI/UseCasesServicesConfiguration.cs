using System.Reflection;
using Application.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure.DI;

public static class UseCasesServicesConfiguration
{
    public static IServiceCollection AddUseCases(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.TryAddScoped<ISender, Sender>();
        MassTransitServicesConfiguration.AddConsumersAssemblies(assemblies);

        return services;
    }
}