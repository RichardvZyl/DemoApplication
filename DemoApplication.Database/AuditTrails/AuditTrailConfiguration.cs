using DemoApplication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApplication.Database;

public sealed class AuditTrailConfiguration : IEntityTypeConfiguration<AuditTrail>
{

    public void Configure(EntityTypeBuilder<AuditTrail> builder)
    {
        builder.ToTable("AuditTrail", "AuditTrails");

        builder.HasKey(tab => tab.Id);

        builder.Property(tab => tab.Id).ValueGeneratedOnAdd().IsRequired(); //.ValueGeneratedOnAdd() //.ValueGeneratedOnAdd()
        builder.Property(tab => tab.Contents).IsRequired().IsUnicode(false).HasMaxLength(4000);
        builder.Property(tab => tab.Date).IsRequired();
        builder.Property(tab => tab.DisplayContext).IsUnicode(false).IsRequired().HasMaxLength(300);
        builder.Property(tab => tab.Model).IsRequired().IsUnicode(false).HasMaxLength(100);
        builder.Property(tab => tab.UserId).IsRequired();
    }
}
