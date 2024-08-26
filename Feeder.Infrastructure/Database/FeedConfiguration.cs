using Collector.Infrastructure.Database;
using Feeder.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feeder.Infrastructure.Database;

public class FeedConfiguration : IEntityTypeConfiguration<Feed>
{
    public void Configure(EntityTypeBuilder<Feed> builder)
    {
        builder.ToTable("feeds");

        builder.HasKey(feed => feed.Id);

        builder.Property(e => e.Title).HasMaxLength(Feed.MaxTitleLength);
        builder.Property(e => e.UserId);

        builder.Navigation(e => e.Sources).AutoInclude();
    }
}