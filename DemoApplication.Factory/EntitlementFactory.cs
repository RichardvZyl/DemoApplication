using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Factory;

public static class EntitlementFactory
{
    public static Entitlement Create(EntitlementModel model)
        => new
            (
                model.UserId,
                model.ViewNotifications,
                model.ViewUsers,
                model.SuspendUsers,
                model.StatisticalReport,
                model.AuditReport,
                model.AuthorizeMakerChecker,
                model.EntitlementChange,
                model.AuditLogs
            );

    public static EntitlementExceptions Create(EntitlementExceptionsModel model)
        => new
        (
            model.UserId,
            model.ExpiresOn,
            model.ViewNotifications,
            model.ViewUsers,
            model.SuspendUsers,
            model.StatisticalReport,
            model.AuditReport,
            model.AuthorizeMakerChecker,
            model.EntitlementChange,
            model.AuditLogs
        );

    public static EntitlementExceptions ApplyExceptionsToEntitlement(EntitlementModel entitlement, EntitlementExceptionsModel exceptions)
    {
        var expired = false;
        if (exceptions.ExpiresOn != null) //Check if the exceptions have an expiry date if so determine if they are still valid
            expired = DateTimeOffset.Compare((DateTimeOffset)exceptions.ExpiresOn, DateTimeOffset.UtcNow) < 1;

        if (expired)
            return EntitlementToExceptionsDomainModel(entitlement); //if the exceptions have expired return saved entitlement

        return new
        (
            entitlement.UserId,
            exceptions.ExpiresOn,
            exceptions.ViewNotifications ?? entitlement.ViewNotifications,
            exceptions.ViewUsers ?? entitlement.ViewUsers,
            exceptions.SuspendUsers ?? entitlement.SuspendUsers,
            exceptions.StatisticalReport ?? entitlement.StatisticalReport,
            exceptions.AuditReport ?? entitlement.AuditReport,
            exceptions.AuthorizeMakerChecker ?? entitlement.AuthorizeMakerChecker,
            exceptions.EntitlementChange ?? entitlement.EntitlementChange,
            exceptions.AuditLogs ?? entitlement.AuditLogs
        );
    }

    private static EntitlementExceptions EntitlementToExceptionsDomainModel(EntitlementModel entitlement)
    => new
        (
            entitlement.UserId,
            null,
            entitlement.ViewNotifications,
            entitlement.ViewUsers,
            entitlement.SuspendUsers,
            entitlement.StatisticalReport,
            entitlement.AuditReport,
            entitlement.AuthorizeMakerChecker,
            entitlement.EntitlementChange,
            entitlement.AuditLogs
        );

    public static EntitlementModel EntitlementExceptionsToEntitlementModel(EntitlementExceptions entitlement)
    => new
        (
            entitlement.UserId,
            entitlement.ViewNotifications ?? false,
            entitlement.ViewUsers ?? false,
            entitlement.SuspendUsers ?? false,
            entitlement.StatisticalReport ?? false,
            entitlement.AuditReport ?? false,
            entitlement.AuthorizeMakerChecker ?? false,
            entitlement.EntitlementChange ?? false,
            entitlement.AuditLogs ?? false
        );
}
