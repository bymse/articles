using Microsoft.EntityFrameworkCore;

namespace Application.Extensions;

public static class DbContextExtensions
{
    public static async ValueTask<T> GetEntity<T>(this DbContext context, Ulid id, CancellationToken ct) where T : class
    {
        var entity = await context.FindEntity<T>(id, ct);
        if (entity is null)
        {
            throw new InvalidOperationException($"Entity of type {typeof(T).Name} with ID {id} not found");
        }

        return entity;
    }

    public static ValueTask<T?> FindEntity<T>(this DbContext context, Ulid id, CancellationToken ct) where T : class
    {
        return context.FindAsync<T>([id], ct);
    }
}