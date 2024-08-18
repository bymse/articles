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

        builder.Property(e => e.Email).HasMaxLength(User.MaxEmailLength);

        builder.OwnsOne(e => e.IdPUser, idp =>
        {
            idp.Property(e => e.Id).HasColumnName("idp_user_id").HasMaxLength(IdPUser.MaxLength);
            idp.Property(e => e.Provider).HasColumnName("idp_provider").HasMaxLength(IdPUser.MaxLength);

            idp.HasIndex(e => new { e.Provider, UserId = e.Id }).IsUnique();
        });
    }
}