using Collector.Application.Entities;
using Infrastructure.Ulids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collector.Infrastructure.Database;

public class MailboxConfiguration : IEntityTypeConfiguration<Mailbox>
{
    public void Configure(EntityTypeBuilder<Mailbox> builder)
    {
        builder.HasKey(e => e.Id);
        builder
            .Property(e => e.Id)
            .HasConversion<UlidValueConverter>();
    }
}