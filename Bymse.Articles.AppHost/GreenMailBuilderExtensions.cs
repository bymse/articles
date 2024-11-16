using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Bymse.Articles.AppHost;

public static class GreenMailBuilderExtensions
{
    public static IDistributedApplicationBuilder AddGreenMail(
        this IDistributedApplicationBuilder builder,
        int imapPort,
        string user,
        string password
    )
    {
        var parts = user.Split('@');

        var opts = $"-Dgreenmail.setup.test.all " +
                   $"-Dgreenmail.hostname=0.0.0.0 " +
                   $"-Dgreenmail.users={parts[0]}:{password}@{parts[1]} " +
                   $"-Dgreenmail.users.login=email " +
                   $"-Dgreenmail.verbose";

        builder
            .AddResource(new ContainerResource(ArticlesResources.GreenMail))
            .WithImage("greenmail/standalone", "2.1.0")
            .WithEndpoint(13025, 3025, name: "smtp", scheme: "tcp", isProxied: false)
            .WithEndpoint(imapPort, 3143, name: "imap", scheme: "tcp", isProxied: false)
            .WithEnvironment("GREENMAIL_OPTS", opts);

        return builder;
    }
}