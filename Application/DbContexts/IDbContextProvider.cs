using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DbContexts;

public interface IDbContextProvider<T>
{
    DbContext Get();
}

public class DbContextProvider<T>(IServiceProvider serviceProvider) : IDbContextProvider<T>
{
    public DbContext Get()
    {
        var key = typeof(T).Name;
        return serviceProvider.GetRequiredKeyedService<DbContext>(key);
    }
}