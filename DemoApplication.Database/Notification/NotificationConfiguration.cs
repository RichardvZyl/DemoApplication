using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApplication.Database
{
    public sealed class NotificationConfiguration : IEntityTypeConfiguration<Domain.Notification>
    {
        public void Configure(EntityTypeBuilder<Domain.Notification> builder)
        {
            builder.ToTable("Notifications", "DemoApplication");

            builder.HasKey(notification => notification.Id);

            builder.Property(notification => notification.Id).ValueGeneratedOnAdd().IsRequired(); //.ValueGeneratedOnAdd()
            builder.Property(notification => notification.Originator).IsRequired(); //.HasMaxLength(50).IsUnicode(false); //changed to Guid
            builder.Property(notification => notification.Description).IsUnicode(false).IsRequired().HasMaxLength(100);
            builder.Property(notification => notification.Date).IsRequired(); //.HasDefaultValueSql("GETDATE()") //.HasDefaultValue(DateTimeOffset.UtcNow)
            builder.Property(notification => notification.Read).HasDefaultValue(false);
            builder.Property(notification => notification.SeenAt).IsRequired(false);
            builder.Property(notification => notification.SeenBy).HasMaxLength(150); //Encrypted email address
            builder.Property(notification => notification.ForRole).IsRequired();
            builder.Property(notification => notification.Entity).IsRequired();
            builder.Property(notification => notification.RelatedId).IsRequired();
        }
    }
}
