using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bymse.Articles.Database;

[UsedImplicitly]
public class ArticlesDbContextFactory : IDesignTimeDbContextFactory<ArticlesDbContext>
{
    public ArticlesDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ArticlesDbContext>()
            .UseNpgsql()
            .UseSnakeCaseNamingConvention()
            .Options;

        return new ArticlesDbContext(options);
    }
}