using Bymse.Articles.Database;
using Bymse.Articles.DbMigrator;
using Infrastructure.ServicesConfiguration;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();

builder.Services
    .AddPostgresDbContext<ArticlesDbContext>()
    .AddHostedService<DbMigratorWorker>();

builder.EnrichNpgsqlDbContext<ArticlesDbContext>();

var host = builder.Build();

host.Run();