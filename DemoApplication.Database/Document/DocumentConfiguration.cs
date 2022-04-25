using DemoApplication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApplication.Database;

public sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents", "Authorizations");

        builder.HasKey(tab => tab.Id);

        builder.Property(tab => tab.Id).IsRequired();
        //builder.Property(tab => tab.RelatedId).IsRequired();
        //builder.Property(tab => tab.SequenceNumber).IsRequired();
        builder.Property(tab => tab.Name).IsRequired().IsUnicode(false).HasMaxLength(50);
        //builder.Property(tab => tab.DocumentSubject).IsUnicode(false).IsRequired().HasMaxLength(100);
        //builder.Property(tab => tab.IsFreeText).IsRequired();
        builder.Property(tab => tab.Contents).IsRequired();
        //builder.Property(tab => tab.DateTimeOffsetLoaded).IsRequired();
        //builder.Property(tab => tab.UserId).IsRequired();

    }
}
