using System.Linq.Expressions;
using Collector.Integration;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Collector.Infrastructure.Database;

public class CollectorSourceIdConverter : ValueConverter<CollectorSourceId, Ulid>
{
    public CollectorSourceIdConverter() : base(e => e.Value, e => new CollectorSourceId(e))
    {
    }
}