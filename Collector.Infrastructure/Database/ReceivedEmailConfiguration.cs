using System.Collections.Specialized;
using System.Text.Json;
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


        builder
            .Property(e => e.Headers)
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, JsonSerializerOptions.Default),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, JsonSerializerOptions.Default) ??
                     new Dictionary<string, string>()
            );

        builder
            .HasIndex(e => new { e.Uid, e.UidValidity })
            .IsUnique();
    }
}