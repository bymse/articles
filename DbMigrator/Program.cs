using Collector.Infrastructure.Database;
using DbMigrator;
using Feeder.Infrastructure.Database;
using Identity.Infrastructure.Database;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();

builder.Services
    .AddHostedService<DbMigratorWorker>();

builder.AddNpgsqlDbContext<IdentityDbContext>(IdentityDbContext.ConnectionName);
builder.AddNpgsqlDbContext<CollectorDbContext>(CollectorDbContext.ConnectionName);
builder.AddNpgsqlDbContext<FeederDbContext>(FeederDbContext.ConnectionName);

var host = builder.Build();

host.Run();