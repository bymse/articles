using Bymse.Articles.Workers.Collector;
using Collector.Infrastructure;
using Collector.Infrastructure.Database;
using Feeder.Infrastructure;
using Feeder.Infrastructure.Database;
using Identity.Infrastructure;
using Identity.Infrastructure.Database;
using Infrastructure.ServicesConfiguration;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<ReceiveEmailsBackgroundWorker>();

var smth = builder.Configuration;

builder.AddServiceDefaults();

builder
    .Services
    .AddIdentityServices()
    .AddFeederServices()
    .AddCollectorServices();

builder
    .AddMassTransitInfrastructure();

builder.EnrichNpgsqlDbContext<IdentityDbContext>();
builder.EnrichNpgsqlDbContext<CollectorDbContext>();
builder.EnrichNpgsqlDbContext<FeederDbContext>();

var host = builder.Build();
host.Run();