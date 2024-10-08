using System.Reflection;
using Application.Contexts;
using Application.Mediator;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.ServicesConfiguration;

public static class MassTransitServicesConfiguration
{
    private static readonly List<Assembly> Assemblies = new();

    internal static void AddConsumersAssemblies(Assembly[] assemblies)
    {
        Assemblies.AddRange(assemblies);
    }

    public static IServiceCollection AddMassTransitInfrastructure(this IHostApplicationBuilder builder)
    {
        var assemblies = Assemblies.ToArray();

        builder.AddRabbitMQClient("rmq-masstransit");

        builder.Services
            .AddScoped<ConsumeContextManager>()
            .AddScoped<IConsumeContextProvider, ConsumeContextManager>();

        builder.Services.AddMediator(x =>
        {
            x.AddConsumers(assemblies);
            x.ConfigureMediator((context, cfg) =>
            {
                cfg.UseConsumeFilter(typeof(UseCaseValidationFilter<>), context,
                    e => { e.Include(r => r.IsAssignableTo(typeof(IUseCase))); });
                cfg.UseConsumeFilter(typeof(UseCaseCommitFilter<>), context,
                    e => { e.Include(r => r.IsAssignableTo(typeof(IUseCase))); });

                cfg.UseConsumeFilter(typeof(MassTransitConsumeContextFilter<>), context);

                cfg.UseInMemoryOutbox(context);
            });
        });

        return builder.Services
                .AddMassTransit(x =>
                {
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        var configuration = context.GetRequiredService<IConfiguration>();
                        var connectionStringRaw = configuration.GetConnectionString("rmq-masstransit") ??
                                                  throw new Exception("Connection string not found for RabbitMQ");
                        var csUri = new Uri(connectionStringRaw);
                        cfg.Host(csUri);

                        cfg.UseConsumeFilter(typeof(MassTransitConsumeContextFilter<>), context);
                    });
                })
            ;
    }
}