using System.Reflection;
using Application.Di;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Di;

public static class AutoRegistrationHelper
{
    public static IServiceCollection AddAutoRegistrations(this IServiceCollection services, params Assembly[] assembly)
    {
        foreach (var asm in assembly)
        {
            foreach (var type in asm.GetTypes().Where(e => e is { IsClass: true, IsAbstract: false }))
            {
                if (type.GetCustomAttribute<AutoRegistrationAttribute>() is not null)
                {
                    services.AddScoped(type);
                }

                foreach (var @interface in type.GetInterfaces())
                {
                    services.AddScoped(@interface, type);
                }
            }
        }

        return services;
    }
}