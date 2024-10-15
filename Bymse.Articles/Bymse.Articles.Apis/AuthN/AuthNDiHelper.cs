using Microsoft.AspNetCore.Authentication;

namespace Bymse.Articles.Apis.AuthN;

public static class AuthNDiHelper
{
    public static IServiceCollection AddStubAuthN(this IServiceCollection services)
    {
        services
            .AddAuthentication(StubAuthenticationHandler.SchemeName)
            .AddScheme<AuthenticationSchemeOptions, StubAuthenticationHandler>(
                StubAuthenticationHandler.SchemeName, _ => { });

        return services;
    }
}