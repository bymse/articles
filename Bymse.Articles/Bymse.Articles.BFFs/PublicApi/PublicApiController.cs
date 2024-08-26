using System.Security.Claims;
using Identity.Integration;
using Microsoft.AspNetCore.Mvc;

namespace Bymse.Articles.BFFs.PublicApi;

[Route("public-api")]
[ApiController]
public class PublicApiController : ControllerBase
{
    protected IdentityUserId UserId => new(
        Ulid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier))
    );
}