using System.Collections.Concurrent;
using Application.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DbContexts;

public interface IUseCaseDbContextProvider
{
    DbContext GetFor<T>() where T : IUseCase;
}

public class UseCaseDbContextProvider(IServiceProvider serviceProvider) : IUseCaseDbContextProvider
{
    public DbContext GetFor<T>() where T : IUseCase
    {
        var key = typeof(T).Namespace!.Split('.').First();
        return serviceProvider.GetRequiredKeyedService<DbContext>(key);
    }
}