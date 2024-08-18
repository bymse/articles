using Collector.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collector.Infrastructure.Database;

public class ConfirmedSourceConfiguration : IEntityTypeConfiguration<ConfirmedSource>
{
    public void Configure(EntityTypeBuilder<ConfirmedSource> builder)
    {
        builder.HasBaseType<Source>();

        builder.Property(e => e.ConfirmedAt);

        builder.ComplexProperty(e => e.Receiver, r =>
        {
            r.Property(e => e.Email).HasColumnName("receiver_email"); 
        });

        builder.HasIndex(e => e.Receiver.Email).IsUnique();
    }
}