using System.Reflection;
using Application.Consumers;
using Application.Contexts;
using Application.Events;
using Application.Mediator;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.ServicesConfiguration;

public static class MassTransitServicesConfiguration
{
    private static readonly List<Assembly> MediatorAssemblies = new();
    private static readonly List<Assembly> ConsumerAssemblies = new();

    internal static void AddMediatorAssemblies(params Assembly[] assemblies)
    {
        MediatorAssemblies.AddRange(assemblies);
    }

    public static IServiceCollection AddConsumers(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        ConsumerAssemblies.AddRange(assemblies);

        return services;
    }

    public static IServiceCollection AddMassTransitInfrastructure(this IHostApplicationBuilder builder,
        bool addConsumers = false)
    {
        builder.AddRabbitMQClient("rmq-masstransit");

        builder.Services
            .AddScoped<ConsumeContextManager>()
            .AddScoped<IEventPublisher, EventPublisher>();

        builder.Services.AddMediator(x =>
        {
            x.AddConsumers(e =>
            {
                var baseType = e.BaseType?.GetGenericTypeDefinition();
                return baseType == typeof(UseCaseHandler<>) || baseType == typeof(UseCaseHandler<,>);
            }, MediatorAssemblies.ToArray());

            x.ConfigureMediator((context, cfg) =>
            {
                cfg.UseConsumeFilter(typeof(UseCaseValidationFilter<>), context,
                    e => { e.Include(r => r.IsAssignableTo(typeof(IUseCase))); });
                cfg.UseConsumeFilter(typeof(UseCaseCommitFilter<>), context,
                    e => { e.Include(r => r.IsAssignableTo(typeof(IUseCase))); });

                cfg.UseConsumeFilter(typeof(MassTransitConsumeContextFilter<>), context);
            });
        });

        return builder.Services
                .AddMassTransit(x =>
                {
                    if (addConsumers)
                    {
                        x.AddConsumers(
                            e => e.BaseType?.GetGenericTypeDefinition() == typeof(EventConsumer<>),
                            ConsumerAssemblies.ToArray()
                        );
                    }

                    x.UsingRabbitMq((context, cfg) =>
                    {
                        var configuration = context.GetRequiredService<IConfiguration>();
                        var connectionStringRaw = configuration.GetConnectionString("rmq-masstransit") ??
                                                  throw new Exception("Connection string not found for RabbitMQ");
                        var csUri = new Uri(connectionStringRaw);
                        cfg.Host(csUri);

                        cfg.ConfigureEndpoints(context);
                        cfg.UseConsumeFilter(typeof(MassTransitConsumeContextFilter<>), context);
                    });
                })
            ;
    }
}