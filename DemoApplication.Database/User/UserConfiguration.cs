using DemoApplication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApplication.Database;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "User");

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(user => user.Active).IsRequired();
        builder.Property(user => user.Email).IsRequired();
        builder.HasIndex(user => user.Email).IsUnique();

        builder.OwnsOne(user => user.FullName, ownedBuilder =>
        {
            ownedBuilder.Property(fullName => fullName.Name).HasColumnName(nameof(FullName.Name)).HasMaxLength(50).IsUnicode(false).IsRequired();
            ownedBuilder.Property(fullName => fullName.Surname).HasColumnName(nameof(FullName.Surname)).HasMaxLength(50).IsUnicode(false).IsRequired();
        });

        builder.HasOne(user => user.Auth);
        //builder.HasOne(user => user.Entitlement);
    }
}
