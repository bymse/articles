using System.Security.Claims;
using Bymse.Articles.Apis.AuthN;
using Identity.Integration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bymse.Articles.Apis.Public;

[ApiController]
[Route("public-api/[controller]")]
[ApiExplorerSettings(GroupName = PublicApiConstants.DocumentName)]
[Authorize(AuthenticationSchemes = StubAuthenticationHandler.SchemeName)]
public class PublicApiController : ControllerBase
{
    protected IdentityUserId UserId => new(
        Ulid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
    );
}