using Collector.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Collector.Infrastructure.Database;

public class UnconfirmedSourceConfiguration : IEntityTypeConfiguration<UnconfirmedSource>
{
    public void Configure(EntityTypeBuilder<UnconfirmedSource> builder)
    {
        builder.HasBaseType<Source>();

        builder.Property(e => e.WebPage);
    }
}