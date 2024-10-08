using Bymse.Articles.Database;
using Bymse.Articles.Workers.Collector;
using Collector.Infrastructure;
using Feeder.Infrastructure;
using Identity.Infrastructure;
using Infrastructure.ServicesConfiguration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ReceiveEmailsBackgroundWorker>();

builder.AddServiceDefaults();

builder
    .Services
    .AddIdentityServices()
    .AddFeederServices()
    .AddCollectorServices()
    .AddPostgresDbContext<ArticlesDbContext>();

builder
    .AddMassTransitInfrastructure<ArticlesDbContext>(addConsumers: true, enableOutboxServices: true);

Thread.Sleep(20 * 1000);

var host = builder.Build();
host.Run();