using System.Security.Claims;
using System.Text.Encodings.Web;
using Identity.Application;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Bymse.Articles.Apis.AuthN;

public class StubAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    IOptions<IdentityApplicationSettings> settings,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Stub";

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var stubId = settings.Value.StubUserId;
        if (string.IsNullOrWhiteSpace(stubId))
        {
            return AuthenticateResult.Fail("Stub user ID not set.");
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, stubId)
        };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);

        var ticket = new AuthenticationTicket(principal, SchemeName);

        return AuthenticateResult.Success(ticket);
    }
}