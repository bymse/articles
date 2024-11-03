using Collector.Application.Handlers.GetManualProcessingEmails;
using Microsoft.AspNetCore.Mvc;

namespace Bymse.Articles.Apis.Public.Manual;

[Route("public-api/manual-processing-emails")]
public class ManualProcessingEmailsController : PublicApiController
{
    [HttpGet]
    public async Task<ManualProcessingEmailInfoCollection> GetManualProcessingEmails(
        [FromServices] GetManualProcessingEmailsHandler handler,
        CancellationToken ct
    )
    {
        return await handler.Handle(ct);
    }
}