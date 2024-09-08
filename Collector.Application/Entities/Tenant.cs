namespace Collector.Application.Entities;

public record Tenant(TenantType Type, Ulid Id)
{
    public static Tenant User(Ulid id) => new(TenantType.User, id);
}

public enum TenantType
{
    User,
}