using Collector.Infrastructure.Imap;

var builder = DistributedApplication.CreateBuilder(args);

//set cookie .AspNetCore.Culture=c=en|uic=en to get eng dashboard 

//Bymse.Articles\Bymse.Articles.AppHost: dotnet user-secrets set Parameters:collector-imap-password <value>
//Bymse.Articles\Bymse.Articles.AppHost: dotnet user-secrets set Parameters:collector-imap-username <value>
var collectorImapPassword = builder.AddParameter("collector-imap-password", secret: true);
var collectorImapUsername = builder.AddParameter("collector-imap-username", secret: true);

//Bymse.Articles\Bymse.Articles.AppHost: dotnet user-secrets set Parameters:articles-postgres-password <value>
var postgresPassword = builder.AddParameter("articles-postgres-password", secret: true);

var articlesSql = builder
    .AddPostgres("articles-postgres", password: postgresPassword, port: 15432)
    .WithDataVolume()
    .AddDatabase("pg-articles", "articles");

var rabbitMqPassword = builder.AddParameter("articles-rabbitmq-password", secret: true);
var rabbitMq = builder
    .AddRabbitMQ("articles-rabbitmq", password: rabbitMqPassword, port: 15672)
    .WithManagementPlugin(port: 15673)
    .WithDataVolume();

builder
    .AddProject<Projects.Bymse_Articles_Apis>("Apis")
    .WithExternalHttpEndpoints()
    .WithReference(articlesSql)
    .WithReference(rabbitMq);

builder
    .AddProject<Projects.Bymse_Articles_DbMigrator>("DbMigrator")
    .WithReference(articlesSql);

builder
    .AddProject<Projects.Bymse_Articles_Workers>("Workers")
    .WithReference(articlesSql)
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