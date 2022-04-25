using System;

namespace DemoApplication.Models;

public class EntitlementModel
{
    public EntitlementModel() {  }

    public EntitlementModel
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

    public Guid UserId { get; set; }

    public bool ViewNotifications { get; set; }
    public bool ViewUsers { get; set; }
    public bool SuspendUsers { get; set; }
    public bool StatisticalReport { get; set; }
    public bool AuditReport { get; set; }
    public bool AuthorizeMakerChecker { get; set; }
    public bool EntitlementChange { get; set; }
    public bool AuditLogs { get; set; }
}
