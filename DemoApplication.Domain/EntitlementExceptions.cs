using Abstractions.Domain;
using DemoApplication.Models;

namespace DemoApplication.Domain;

public class EntitlementExceptions : Entity<Guid>
{
    public EntitlementExceptions
    (
        Guid userId,
        DateTimeOffset? expiresOn,
        bool? viewNotifications,
        bool? viewUsers,
        bool? suspendUsers,
        bool? statisticalReport,
        bool? auditReport,
        bool? authorizeMakerChecker,
        bool? entitlementChange,
        bool? auditLogs
    )
    {
        UserId = userId;
        ExpiresOn = expiresOn;
        ViewNotifications = viewNotifications;
        ViewUsers = viewUsers;
        SuspendUsers = suspendUsers;
        StatisticalReport = statisticalReport;
        AuditReport = auditReport;
        AuthorizeMakerChecker = authorizeMakerChecker;
        EntitlementChange = entitlementChange;
        AuditLogs = auditLogs;
    }

    public EntitlementExceptions(Guid id) : base(id) { }

    public Guid UserId { get; private set; }

    public DateTimeOffset? ExpiresOn { get; private set; }

    public bool? ViewNotifications { get; private set; }
    public bool? ViewUsers { get; private set; }
    public bool? SuspendUsers { get; private set; }
    public bool? StatisticalReport { get; private set; }
    public bool? AuditReport { get; private set; }
    public bool? AuthorizeMakerChecker { get; private set; }
    public bool? EntitlementChange { get; private set; }
    public bool? AuditLogs { get; private set; }

    #region Interactions
    public void UpdateExceptions
    (
        EntitlementExceptionsModel model
    )
    {
        if (model.ExpiresOn is not null)
            UpdateeExpiryDate(model.ExpiresOn);

        this.ViewNotifications = model.ViewNotifications;
        this.ViewUsers = model.ViewUsers;
        this.SuspendUsers = model.SuspendUsers;
        this.StatisticalReport = model.StatisticalReport;
        this.AuditReport = model.AuditReport;
        this.AuthorizeMakerChecker = model.AuthorizeMakerChecker;
        this.EntitlementChange = model.EntitlementChange;
        this.AuditReport = model.AuditReport;
        this.EntitlementChange = model.EntitlementChange;
        this.AuditLogs = model.AuditLogs;
    }

    private void UpdateeExpiryDate
    (
        DateTimeOffset? expiresOn
    )
        => this.ExpiresOn = expiresOn;
    #endregion

}
