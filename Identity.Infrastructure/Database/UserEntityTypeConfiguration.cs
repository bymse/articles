using Identity.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Database;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(e => e.Id).HasName("user_id");

        builder.ComplexProperty(e => e.IdP, idp =>
        {
            idp.Property(e => e.UserId).HasColumnName("idp_user_id");
            idp.Property(e => e.Provider).HasColumnName("idp_provider");
        });

        builder.HasIndex(e => new { e.IdP.Provider, e.IdP.UserId }).IsUnique();

        builder.Property(e => e.Email);
    }
}