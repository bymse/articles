using System.Reflection;
using Application.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Infrastructure.DI;

public static class UseCasesDiHelper
{
    public static IServiceCollection AddUseCases(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.TryAddScoped<IUseCaseExecutor, UseCaseExecutor>();
        MassTransitDiHelper.AddConsumersAssemblies(assemblies);

        return services;
    }
}