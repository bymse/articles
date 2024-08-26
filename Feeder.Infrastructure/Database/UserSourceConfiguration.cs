using Feeder.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feeder.Infrastructure.Database;

public class UserSourceConfiguration : IEntityTypeConfiguration<UserSource>
{
    public void Configure(EntityTypeBuilder<UserSource> builder)
    {
        builder.ToTable("user_sources");
        builder.HasKey(x => new { x.UserId, x.SourceId });
    }
}