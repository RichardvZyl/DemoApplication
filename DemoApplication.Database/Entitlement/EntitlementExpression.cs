using System.Linq.Expressions;
using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Database;

public static class EntitlementExpression
{
    public static Expression<Func<Entitlement, bool>> UserId(Guid id) => entitlement => entitlement.UserId == id;

    public static Expression<Func<Entitlement, EntitlementModel>> Model => entitlement => new EntitlementModel
    {
        UserId = entitlement.UserId,

        ViewNotifications = entitlement.ViewNotifications,
        ViewUsers = entitlement.ViewUsers,
        SuspendUsers = entitlement.SuspendUsers,
        StatisticalReport = entitlement.StatisticalReport,
        AuditReport = entitlement.AuditReport,
        AuthorizeMakerChecker = entitlement.AuthorizeMakerChecker,
        EntitlementChange = entitlement.EntitlementChange,
        AuditLogs = entitlement.AuditLogs
    };
}
