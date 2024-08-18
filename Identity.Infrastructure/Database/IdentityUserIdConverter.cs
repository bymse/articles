using Identity.Integration;
using Infrastructure.Ulids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.Infrastructure.Database;

public class IdentityUserIdConverter() : ValueConverter<IdentityUserId, string>(e => e.Value.ToString(),
    e => new IdentityUserId(Ulid.Parse(e)),
    UlidConverters.DefaultHints);