using System.Linq.Expressions;
using Collector.Integration;
using Infrastructure.Ulids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Collector.Infrastructure.Database;

public class CollectorSourceIdConverter : ValueConverter<CollectorSourceId, string>
{
    public CollectorSourceIdConverter() : base(
        e => e.Value.ToString(),
        e => new CollectorSourceId(Ulid.Parse(e)),
        UlidConverters.DefaultHints)
    {
    }
}