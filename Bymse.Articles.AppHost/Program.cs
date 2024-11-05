using Application.Extensions;
using Bymse.Articles.AppHost;

/* BEFORE START
-- add password for postgres
dotnet user-secrets set Parameters:articles-postgres-password <value> --project Bymse.Articles.AppHost

-- add password for rabbitmq
dotnet user-secrets set Parameters:articles-rabbitmq-password <value> --project Bymse.Articles.AppHost

-- add local settings
Create appsettings.Development.json from appsettings.Development.json.template and set your values

set cookie .AspNetCore.Culture=c=en|uic=en to get eng dashboard
*/

var noVolumes = args.Contains("--no-volumes");
var needGreenmail = args.Contains("--run-green-mail");

var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("articles-postgres-password", secret: true);
var articlesSql = builder
    .AddPostgres(ArticlesResources.Postgres, password: postgresPassword, port: 15432)
    .If(!noVolumes, e => e.WithDataVolume())
    .AddDatabase("pg-articles", "articles");

var rabbitMqPassword = builder.AddParameter("articles-rabbitmq-password", secret: true);
var rabbitMq = builder
    .AddRabbitMQ(ArticlesResources.RabbitMq, password: rabbitMqPassword, port: 15672)
    .WithManagementPlugin(port: 15673)
    .If(!noVolumes, e => e.WithDataVolume());

var apis = builder
    .AddProject<Projects.Bymse_Articles_Apis>(ArticlesResources.Apis)
    .WithExternalHttpEndpoints()
    .WithReference(articlesSql)
    .WithReference(rabbitMq);

var dbMigrator = builder
    .AddProject<Projects.Bymse_Articles_DbMigrator>(ArticlesResources.DbMigrator)
    .WithReference(articlesSql);

var workers = builder
    .AddProject<Projects.Bymse_Articles_Workers>(ArticlesResources.Workers)
    .WithReference(articlesSql)
    .WithReference(rabbitMq);

ConfigurationHelper.PopulateEnvironment(builder, apis, dbMigrator, workers);
ImapHelper.AddImap(builder, workers, needGreenmail);

builder.Build().Run();