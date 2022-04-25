using DemoApplication.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApplication.Database;

public sealed class EntitlementExceptionsConfiguration : IEntityTypeConfiguration<EntitlementExceptions>
{
    public void Configure(EntityTypeBuilder<EntitlementExceptions> builder)
    {
        builder.ToTable("Entitlement", "EntitlementExceptions");

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
