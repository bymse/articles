using Collector.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collector.Infrastructure.Database;

public class ReceivedEmailConfiguration : IEntityTypeConfiguration<ReceivedEmail>
{
    public void Configure(EntityTypeBuilder<ReceivedEmail> builder)
    {
        builder
            .HasOne<Mailbox>()
            .WithMany()
            .HasForeignKey(e => e.MailboxId);


        builder.OwnsOne(e => e.Headers, e => e.ToJson());
    }
}