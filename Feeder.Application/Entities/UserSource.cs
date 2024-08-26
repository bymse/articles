using Collector.Integration;
using Identity.Integration;

namespace Feeder.Application.Entities;

public class UserSource(IdentityUserId userId, CollectorSourceId sourceId)
{
    public IdentityUserId UserId { get; protected set; } = userId;
    public CollectorSourceId SourceId { get; protected set; } = sourceId;
}