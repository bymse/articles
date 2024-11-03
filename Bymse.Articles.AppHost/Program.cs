using Application.Extensions;
using Bymse.Articles.AppHost;
using Collector.Application.Settings;
using Collector.Infrastructure.Imap;

var noVolumes = args.Contains("--no-volumes");

var builder = DistributedApplication.CreateBuilder(args);

//set cookie .AspNetCore.Culture=c=en|uic=en to get eng dashboard 

//Bymse.Articles\Bymse.Articles.AppHost: dotnet user-secrets set Parameters:collector-imap-password <value>
//Bymse.Articles\Bymse.Articles.AppHost: dotnet user-secrets set Parameters:collector-imap-username <value>
var collectorImapPassword = builder.AddParameter("collector-imap-password", secret: true);
var collectorImapUsername = builder.AddParameter("collector-imap-username", secret: true);

//Bymse.Articles\Bymse.Articles.AppHost: dotnet user-secrets set Parameters:collector-root-receiver <value>
var collectorRootReceiver = builder.AddParameter("collector-root-receiver");

//Bymse.Articles\Bymse.Articles.AppHost: dotnet user-secrets set Parameters:articles-postgres-password <value>
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

var needGreenmail = args.Contains("--run-green-mail");

int imapPort;
string imapHost;
bool useSsl;
if (needGreenmail)
{
    imapPort = 3143;
    imapHost = "localhost";
    useSsl = false;
    builder
        .AddResource(new ContainerResource(ArticlesResources.GreenMail))
        .WithImage("greenmail/standalone", "2.1.0")
        .WithEndpoint(13025, 3025, name: "smtp", scheme: "tcp", isProxied: false)
        .WithEndpoint(imapPort, imapPort, name: "imap", scheme: "tcp", isProxied: false)
        .WithEnvironment("GREENMAIL_OPTS",
            "-Dgreenmail.setup.test.all -Dgreenmail.hostname=0.0.0.0 -Dgreenmail.auth.disabled -Dgreenmail.verbose -Dgreenmail.users=collector1:collector1")
        ;
}
else
{
    imapPort = 993;
    imapHost = "imap.mail.ru";
    useSsl = true;
}

builder
    .AddProject<Projects.Bymse_Articles_Apis>(ArticlesResources.Apis)
    .WithExternalHttpEndpoints()
    .WithReference(articlesSql)
    .WithReference(rabbitMq)
    .WithEnvironment(
        $"{CollectorApplicationSettings.Path}:{nameof(CollectorApplicationSettings.RootReceiver)}",
        collectorRootReceiver
    );

builder
    .AddProject<Projects.Bymse_Articles_DbMigrator>(ArticlesResources.DbMigrator)
    .WithReference(articlesSql);

builder
    .AddProject<Projects.Bymse_Articles_Workers>(ArticlesResources.Workers)
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
    .WithEnvironment(
        $"{ImapEmailServiceSettings.Path}:{nameof(ImapEmailServiceSettings.Port)}",
        imapPort.ToString()
    )
    .WithEnvironment(
        $"{ImapEmailServiceSettings.Path}:{nameof(ImapEmailServiceSettings.Hostname)}",
        imapHost
    )
    .WithEnvironment(
        $"{ImapEmailServiceSettings.Path}:{nameof(ImapEmailServiceSettings.UseSsl)}",
        useSsl.ToString()
    );

builder.Build().Run();