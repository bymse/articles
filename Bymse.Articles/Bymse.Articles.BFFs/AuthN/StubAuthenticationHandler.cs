using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Bymse.Articles.BFFs.AuthN;

public class StubAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    IConfiguration configuration,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Stub";

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var stubId = configuration.GetValue<string>("StubUserId");
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