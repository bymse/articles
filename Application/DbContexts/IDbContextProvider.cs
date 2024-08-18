using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DbContexts;

public interface IDbContextProvider<T>
{
    DbContext Get();
}

public class DbContextProvider<T> : IDbContextProvider<T>
{
    private readonly IKeyedServiceProvider keyedServiceProvider;

    public DbContextProvider(IKeyedServiceProvider keyedServiceProvider)
    {
        this.keyedServiceProvider = keyedServiceProvider;
    }

    public DbContext Get()
    {
        var key = typeof(T).Namespace!.Split(".").First();
        return keyedServiceProvider.GetRequiredKeyedService<DbContext>(key);
    }
}