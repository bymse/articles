using Feeder.Application.Entities;
using Infrastructure.Ulids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Feeder.Infrastructure.Database;

public class FeedIdConverter() : ValueConverter<FeedId, string>(e => e.Value.ToString(),
    e => new FeedId(Ulid.Parse(e)),
    UlidConverters.DefaultHints);