using Collector.Infrastructure.Database;
using DbMigrator;
using Feeder.Infrastructure.Database;
using Identity.Infrastructure.Database;
using Infrastructure.ServicesConfiguration;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();

builder.Services
    .AddPostgresDbContext<IdentityDbContext>()
    .AddPostgresDbContext<CollectorDbContext>()
    .AddPostgresDbContext<FeederDbContext>()
    .AddHostedService<DbMigratorWorker>();

builder.EnrichNpgsqlDbContext<IdentityDbContext>();
builder.EnrichNpgsqlDbContext<CollectorDbContext>();
builder.EnrichNpgsqlDbContext<FeederDbContext>();

var host = builder.Build();

host.Run();