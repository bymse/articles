using Collector.Infrastructure.Imap;
using Microsoft.Extensions.Configuration;

namespace Bymse.Articles.AppHost;

public class ImapHelper
{
    public static void AddImap(IDistributedApplicationBuilder applicationBuilder,
        IResourceBuilder<ProjectResource> resourceBuilder,
        bool needGreenmail
    )
    {
        if (needGreenmail)
        {
            const int imapPort = 3143;

            var settings = applicationBuilder.Configuration
                .GetRequiredSection(ImapEmailServiceSettings.Path)
                .Get<ImapEmailServiceSettings>()!;

            applicationBuilder.AddGreenMail(imapPort,
                settings.Username,
                settings.Password
            );

            resourceBuilder
                .WithEnvironment(
                    $"{ImapEmailServiceSettings.Path}:{nameof(ImapEmailServiceSettings.Port)}",
                    imapPort.ToString()
                )
                .WithEnvironment(
                    $"{ImapEmailServiceSettings.Path}:{nameof(ImapEmailServiceSettings.Hostname)}",
                    "localhost"
                )
                .WithEnvironment(
                    $"{ImapEmailServiceSettings.Path}:{nameof(ImapEmailServiceSettings.UseSsl)}",
                    "false"
                );
        }
    }
}