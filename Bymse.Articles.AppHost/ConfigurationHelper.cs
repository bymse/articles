using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.Configuration;

namespace Bymse.Articles.AppHost;

public static class ConfigurationHelper
{
    public static void PopulateEnvironment(IDistributedApplicationBuilder builder,
        params IResourceBuilder<ProjectResource>[] resourceBuilders)
    {
        var pairs = builder.Configuration.AsEnumerable().ToArray();

        foreach (var resourceBuilder in resourceBuilders)
        {
            foreach (var pair in pairs)
            {
                resourceBuilder.WithEnvironment(pair.Key, pair.Value);
            }
        }
    }
}