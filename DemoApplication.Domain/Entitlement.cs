using Abstractions.Domain;
using DemoApplication.Models;

namespace DemoApplication.Domain;

public class Entitlement : Entity<Guid>
{
    public Entitlement
    (
        Guid userId,
        bool viewNotifications,
        bool viewUsers,
        bool suspendUsers,
        bool statisticalReport,
        bool auditReport,
        bool authorizeMakeChecker,
        bool entitlementChange,
        bool auditLogs
    )
    {
        this.UserId = userId;
        this.ViewNotifications = viewNotifications;
        this.ViewUsers = viewUsers;
        this.SuspendUsers = suspendUsers;
        this.StatisticalReport = statisticalReport;
        this.AuditReport = auditReport;
        this.AuthorizeMakerChecker = authorizeMakeChecker;
        this.EntitlementChange = entitlementChange;
        this.AuditLogs = auditLogs;
    }

    public Entitlement(Guid id) : base(id) { }

    public override Guid Id => UserId;

    public Guid UserId { get; private set; }

    public bool ViewNotifications { get; private set; }
    public bool ViewUsers { get; private set; }
    public bool SuspendUsers { get; private set; }
    public bool StatisticalReport { get; private set; }
    public bool AuditReport { get; private set; }
    public bool AuthorizeMakerChecker { get; private set; }
    public bool EntitlementChange { get; private set; }
    public bool AuditLogs { get; private set; }

    public void Update(EntitlementModel entitlementModel)
    {
        this.UserId = entitlementModel.UserId;
        this.ViewNotifications = entitlementModel.ViewNotifications;
        this.SuspendUsers = entitlementModel.SuspendUsers;
        this.StatisticalReport = entitlementModel.StatisticalReport;
        this.AuditReport = entitlementModel.AuditReport;
        this.AuthorizeMakerChecker = entitlementModel.AuthorizeMakerChecker;
        this.EntitlementChange = entitlementModel.EntitlementChange;
        this.AuditLogs = entitlementModel.AuditLogs;
    }
}
