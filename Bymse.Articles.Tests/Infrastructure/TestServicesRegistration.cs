using Bymse.Articles.Tests.Actions;
using Bymse.Articles.Tests.Emails;

namespace Bymse.Articles.Tests.Infrastructure;

public static class TestServicesRegistration
{
    public static IServiceCollection AddArticlesTestServices(this IServiceCollection services, Func<IServiceProvider, IEmailSender> emailSenderFactory)
    {
        return services
            .AddSingleton<IArticlesActions, ArticlesActions>()
            .AddSingleton<ICollectorActions, CollectorActions>()
            .AddSingleton<IExternalSystemActions, ExternalSystemActions>()
            .AddSingleton(emailSenderFactory);
    }
}