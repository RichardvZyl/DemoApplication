using System.Security.Claims;

namespace DemoApplication.Entitlement;

public static class Claims
{
    public const string ViewNotifications = "ViewNotifications";
    public const string ViewUsers = "ViewUsers";
    public const string SuspendUsers = "SuspendUsers";
    public const string StatisticalReport = "StatisticalReport";
    public const string AuditReport = "AuditReport";
    public const string AuthorizeMakerChecker = "AuthorizeMakerChecker";
    public const string EntitlementChange = "EntitlementChange";
    public const string AuditLogs = "AuditLogs";
}

public static class ClaimTypes
{
    public const string Create = "Create";
    public const string Read = "Read";
    public const string Update = "Update";
    public const string Delete = "Delete";

    public const string Report = "Report"; //should this fall under read? // I think this should be a seperate type
    public const string Admin = "Admin";
}

public static class DemoApplicationClaims
{
    public static Claim ViewNotifications => new(ClaimTypes.Read, Claims.ViewNotifications);
    public static Claim ViewUsers => new(ClaimTypes.Read, Claims.ViewUsers);
    public static Claim SuspendUsers => new(ClaimTypes.Update, Claims.SuspendUsers);
    public static Claim StatisticalReport => new(ClaimTypes.Report, Claims.StatisticalReport);
    public static Claim AuditReport => new(ClaimTypes.Report, Claims.AuditReport);
    public static Claim AuthorizeMakerChecker => new(ClaimTypes.Admin, Claims.AuthorizeMakerChecker);
    public static Claim EntitlementChange => new(ClaimTypes.Admin, Claims.EntitlementChange);
    public static Claim AuditLogs => new(ClaimTypes.Admin, Claims.AuditLogs);
}
