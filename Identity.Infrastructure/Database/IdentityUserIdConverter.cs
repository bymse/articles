using System.Linq.Expressions;
using Identity.Integration;
using Infrastructure.Ulids;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.Infrastructure.Database;

public class IdentityUserIdConverter : ValueConverter<IdentityUserId, string>
{
    public IdentityUserIdConverter() : base(
        e => e.Value.ToString(),
        e => new IdentityUserId(Ulid.Parse(e)),
        UlidConverters.DefaultHints)
    {
    }
}