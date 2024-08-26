using Collector.Infrastructure.Database;
using Feeder.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feeder.Infrastructure.Database;

public class FeedSourceConfiguration : IEntityTypeConfiguration<FeedSource>
{
    public void Configure(EntityTypeBuilder<FeedSource> builder)
    {
        builder.ToTable("feed_sources");

        builder.HasKey(e => new { e.FeedId, e.SourceId });

        builder.Property(e => e.FeedId);
        builder.Property(e => e.SourceId);

        builder.HasOne<Feed>()
            .WithMany(e => e.Sources)
            .HasForeignKey(e => e.FeedId);
    }
}