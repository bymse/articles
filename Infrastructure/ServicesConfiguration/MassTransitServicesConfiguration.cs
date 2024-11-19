using System.Reflection;
using Application.Consumers;
using Application.Contexts;
using Application.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.ServicesConfiguration;

public static class MassTransitServicesConfiguration
{
    private static readonly List<Assembly> ConsumerAssemblies = new();

    public static IServiceCollection AddConsumers(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        ConsumerAssemblies.AddRange(assemblies);

        return services;
    }

    public static IServiceCollection AddMassTransitInfrastructure<TDbContext>(this IHostApplicationBuilder builder,
        bool addConsumers = false,
        bool enableOutboxServices = false) where TDbContext : DbContext
    {
        builder.AddRabbitMQClient("articles-rabbitmq");

        builder.Services
            .AddScoped<ConsumeContextManager>()
            .AddScoped<IPublisher, Publisher>();

        return builder.Services
            .AddMassTransit(x =>
            {
                if (addConsumers)
                {
                    x.AddConsumers(
                        e => e.BaseType?.IsGenericType == true
                             && e.BaseType?.GetGenericTypeDefinition() == typeof(EventConsumer<>),
                        ConsumerAssemblies.ToArray()
                    );
                }

                x.UsingArticlesRabbitMq();

                x.AddEntityFrameworkOutbox<TDbContext>(e =>
                {
                    e.UsePostgres();
                    e.QueryDelay = TimeSpan.FromSeconds(5);

                    if (!enableOutboxServices)
                    {
                        e.DisableInboxCleanupService();
                    }


                    e.UseBusOutbox(r =>
                    {
                        if (!enableOutboxServices)
                        {
                            r.DisableDeliveryService();
                        }
                    });
                });
            });
    }

    public static void UsingArticlesRabbitMq(this IBusRegistrationConfigurator configurator,
        Func<string?>? connectionStringFactory = null)
    {
        configurator.UsingRabbitMq((context, cfg) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var connectionStringRaw = connectionStringFactory?.Invoke() ??
                                      configuration.GetConnectionString("articles-rabbitmq") ??
                                      throw new Exception("Connection string not found for RabbitMQ");
            var csUri = new Uri(connectionStringRaw);
            cfg.Host(csUri);

            cfg.ConfigureEndpoints(context);

            cfg.UseConsumeFilter(typeof(MassTransitConsumeContextFilter<>), context);
        });
    }
}