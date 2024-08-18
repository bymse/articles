using System.Linq.Expressions;
using Identity.Integration;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.Infrastructure.Database;

public class IdentityUserIdConverter : ValueConverter<IdentityUserId, Ulid>
{
    public IdentityUserIdConverter() : base(e => e.Value, e => new IdentityUserId(e))
    {
    }
}