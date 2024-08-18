using Identity.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Database;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email).HasMaxLength(100);

        builder.OwnsOne(e => e.IdPUser, idp =>
        {
            idp.Property(e => e.Id).HasColumnName("idp_user_id").HasMaxLength(100);
            idp.Property(e => e.Provider).HasColumnName("idp_provider").HasMaxLength(100);

            idp.HasIndex(e => new { e.Provider, UserId = e.Id }).IsUnique();
        });
    }
}