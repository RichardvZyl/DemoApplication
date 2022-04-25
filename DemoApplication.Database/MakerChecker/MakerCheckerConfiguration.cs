using DemoApplication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApplication.Database;

public sealed class MakerCheckerConfiguration : IEntityTypeConfiguration<MakerChecker>
{
    public void Configure(EntityTypeBuilder<MakerChecker> builder)
    {
        builder.ToTable("MakerChecker", "Auth");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired(); //.ValueGeneratedOnAdd()

        builder.Property(x => x.Action).IsRequired(); //.IsUnicode(false).HasMaxLength(100); //Changed to enum
        builder.Property(x => x.MakerDate).IsRequired();
        builder.Property(x => x.MakerUser).IsRequired(); //.HasMaxLength(200);
        builder.Property(x => x.Motivation).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Model).IsRequired().IsUnicode(false);

        //builder.Property(x => x.FileIds);

        builder.Property(x => x.CheckerDate);
        builder.Property(x => x.CheckerUser).HasMaxLength(200);

        builder.Ignore(x => x.Files);

        // builder.HasMany(x => x.Files);
    }
}
