using Collector.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collector.Infrastructure.Database;

public class ManualProcessingEmailConfiguration : IEntityTypeConfiguration<ManualProcessingEmail>
{
    public void Configure(EntityTypeBuilder<ManualProcessingEmail> builder)
    {
        builder
            .HasOne<ReceivedEmail>()
            .WithMany()
            .HasForeignKey(e => e.ReceivedEmailId);
    }
}