using Collector.Infrastructure.Database;
using Feeder.Infrastructure.Database;
using Identity.Infrastructure.Database;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("ArticlesPostgres");

var identitySql = postgres.AddDatabase(IdentityDbContext.ConnectionName);
var feederSql = postgres.AddDatabase(FeederDbContext.ConnectionName);
var collectorSql = postgres.AddDatabase(CollectorDbContext.ConnectionName);

builder
    .AddProject<Projects.Bymse_Articles_BFFs>("BFFs")
    .WithExternalHttpEndpoints();

builder
    .AddProject<Projects.DbMigrator>("DbMigrator")
    .WithExternalHttpEndpoints()
    .WithReference(identitySql)
    .WithReference(feederSql)
    .WithReference(collectorSql);


builder.Build().Run();