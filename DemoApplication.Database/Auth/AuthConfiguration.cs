using DemoApplication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApplication.Database;

public sealed class AuthConfiguration : IEntityTypeConfiguration<Auth>
{
    public void Configure(EntityTypeBuilder<Auth> builder)
    {
        builder.ToTable("Auths", "Auth");

        builder.HasKey(auth => auth.Id);

        builder.HasIndex(auth => auth.Email).IsUnique();

        builder.Property(auth => auth.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(auth => auth.Email).HasMaxLength(100).IsUnicode(false).IsRequired();
        builder.Property(auth => auth.Password).HasMaxLength(500).IsUnicode(false).IsRequired();
        builder.Property(auth => auth.Salt).HasMaxLength(500).IsUnicode(false).IsRequired();
        builder.Property(auth => auth.Role).IsRequired();
    }
}
