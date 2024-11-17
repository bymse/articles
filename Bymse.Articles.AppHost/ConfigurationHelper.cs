using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.Configuration;

namespace Bymse.Articles.AppHost;

public static class ConfigurationHelper
{
    private static string[] AllowedPrefixes = ["collector", "identity"];
    
    public static void PopulateEnvironment(IDistributedApplicationBuilder builder,
        params IResourceBuilder<ProjectResource>[] resourceBuilders)
    {
        var pairs = builder.Configuration.AsEnumerable()
            .Where(e => AllowedPrefixes.Any(p => e.Key.StartsWith(p)))
            .ToArray();

        foreach (var resourceBuilder in resourceBuilders)
        {
            foreach (var pair in pairs)
            {
                resourceBuilder.WithEnvironment(pair.Key, pair.Value);
            }
        }
    }
}