using System.Security.Claims;
using Bymse.Articles.BFFs.AuthN;
using Identity.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bymse.Articles.BFFs.PublicApi;

[Authorize(AuthenticationSchemes = StubAuthenticationHandler.SchemeName)]
[Route("public-api")]
[ApiController]
public class PublicApiController : ControllerBase
{
    protected IdentityUserId UserId => new(
        Ulid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
    );
}