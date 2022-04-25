using System;
using System.Linq.Expressions;
using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Database;

public static class EntitlementExceptionsExpression
{
    public static Expression<Func<EntitlementExceptions, bool>> UserId(Guid id) => entitlement => entitlement.UserId == id;

    public static Expression<Func<EntitlementExceptions, EntitlementExceptionsModel>> Model => entitlement => new EntitlementExceptionsModel
    {
        UserId = entitlement.UserId,
        ExpiresOn = entitlement.ExpiresOn,
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
