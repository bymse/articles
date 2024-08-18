using Collector.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collector.Infrastructure.Database;

public class SourceConfiguration : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.ToTable("sources");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.State);
        builder.Property(e => e.CreatedAt);

        builder
            .HasDiscriminator(e => e.State)
            .HasValue<UnconfirmedSource>(SourceState.Unconfirmed)
            .HasValue<ConfirmedSource>(SourceState.Confirmed);
    }
}