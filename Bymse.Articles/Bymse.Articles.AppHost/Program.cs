using Collector.Application.Settings;
using Collector.Infrastructure.Database;
using Collector.Infrastructure.Imap;
using Feeder.Infrastructure.Database;
using Identity.Infrastructure.Database;

var builder = DistributedApplication.CreateBuilder(args);

//set cookie .AspNetCore.Culture=c=en|uic=en to get eng dashboard 

//Bymse.Articles\Bymse.Articles.AppHost: dotnet user-secrets set Parameters:collector-imap-password <value>
//Bymse.Articles\Bymse.Articles.AppHost: dotnet user-secrets set Parameters:collector-imap-username <value>
var collectorImapPassword = builder.AddParameter("collector-imap-password", secret: true);
var collectorImapUsername = builder.AddParameter("collector-imap-username", secret: true);

//Bymse.Articles\Bymse.Articles.AppHost: dotnet user-secrets set Parameters:articles-postgres-password <value>
var postgresPassword = builder.AddParameter("articles-postgres-password", secret: true);
var postgres = builder
    .AddPostgres("articles-postgres", password: postgresPassword, port: 15432)
    .WithDataVolume();

var identitySql = postgres.AddDatabase($"pg-{IdentityDbContext.Key}", IdentityDbContext.Key);
var feederSql = postgres.AddDatabase($"pg-{FeederDbContext.Key}", FeederDbContext.Key);
var collectorSql = postgres.AddDatabase($"pg-{CollectorDbContext.Key}", CollectorDbContext.Key);

var rabbitMq = builder
    .AddRabbitMQ("rmq-masstransit")
    .WithManagementPlugin()
    .WithDataVolume();

builder
    .AddProject<Projects.Bymse_Articles_BFFs>("BFFs")
    .WithExternalHttpEndpoints()
    .WithReference(identitySql)
    .WithReference(feederSql)
    .WithReference(collectorSql)
    .WithReference(rabbitMq);

builder
    .AddProject<Projects.DbMigrator>("DbMigrator")
    .WithReference(identitySql)
    .WithReference(feederSql)
    .WithReference(collectorSql);

builder
    .AddProject<Projects.Bymse_Articles_Workers>("Workers")
    .WithReference(identitySql)
    .WithReference(feederSql)
    .WithReference(collectorSql)
    .WithReference(rabbitMq)
    .WithEnvironment(
        $"{ImapEmailServiceSettings.Path}:{nameof(ImapEmailServiceSettings.Username)}",
        collectorImapUsername
    )
    .WithEnvironment(
        $"{ImapEmailServiceSettings.Path}:{nameof(ImapEmailServiceSettings.Password)}",
        collectorImapPassword
    )
    ;

builder.Build().Run();