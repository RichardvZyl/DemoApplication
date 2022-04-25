using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DemoApplication.Domain;

namespace GoBills.DAL.Configurations
{
    public sealed class EntitlementConfiguration : IEntityTypeConfiguration<Entitlement>
    {
        public void Configure(EntityTypeBuilder<Entitlement> builder)
        {
            builder.ToTable("Entitlement", "Entitlement");

            builder.HasKey(tab => tab.UserId);

            builder.Property(tab => tab.UserId).IsRequired();
            builder.Property(tab => tab.ViewNotifications);
            builder.Property(tab => tab.ViewUsers);
            builder.Property(tab => tab.SuspendUsers);
            builder.Property(tab => tab.StatisticalReport);
            builder.Property(tab => tab.AuditReport);
            builder.Property(tab => tab.AuthorizeMakerChecker);
            builder.Property(tab => tab.EntitlementChange);
            builder.Property(tab => tab.AuditLogs);
        }
    }
}
