using Collector.Application.Entities;
using Infrastructure.Ulids;
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
        
        builder.OwnsOne(e => e.Receiver, r =>
        {
            r.Property(e => e.Email).HasColumnName("receiver_email");
            r.HasIndex(e => e.Email).IsUnique();
        });

        builder
            .HasDiscriminator(e => e.State)
            .HasValue<UnconfirmedSource>(SourceState.Unconfirmed)
            .HasValue<ConfirmedSource>(SourceState.Confirmed);
    }
}