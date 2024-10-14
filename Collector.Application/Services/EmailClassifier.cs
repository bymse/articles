using Collector.Application.Entities;
using Microsoft.EntityFrameworkCore;

namespace Collector.Application.Services;

public class EmailClassifier(DbContext context)
{
    public async Task<EmailType> Classify(EmailModel email, CancellationToken ct)
    {
        var sourceState = await context
            .Set<Source>()
            .Where(e => e.Receiver.Email == email.ToEmail)
            .Select(e => e.State)
            .Cast<SourceState?>()
            .FirstOrDefaultAsync(ct);

        return sourceState switch
        {
            SourceState.Unconfirmed => EmailType.Confirmation,
            SourceState.Confirmed => EmailType.Articles,
            null => EmailType.Unknown,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}