using Collector.Infrastructure.Database;
using Feeder.Infrastructure.Database;
using Identity.Infrastructure.Database;

var builder = DistributedApplication.CreateBuilder(args);

//set cookie .AspNetCore.Culture=c=en|uic=en to get eng dashboard 

//dotnet user-secrets set Parameters:articles-postgres-password <value>
var postgresPassword = builder.AddParameter("articles-postgres-password", secret: true);

var postgres = builder
    .AddPostgres("articles-postgres", password: postgresPassword, port: 15432)
    .WithDataVolume();

var identitySql = postgres.AddDatabase(IdentityDbContext.ConnectionName);
var feederSql = postgres.AddDatabase(FeederDbContext.ConnectionName);
var collectorSql = postgres.AddDatabase(CollectorDbContext.ConnectionName);

builder
    .AddProject<Projects.Bymse_Articles_BFFs>("BFFs")
    .WithExternalHttpEndpoints();

builder
    .AddProject<Projects.DbMigrator>("DbMigrator")
    .WithReference(identitySql)
    .WithReference(feederSql)
    .WithReference(collectorSql);


builder.Build().Run();