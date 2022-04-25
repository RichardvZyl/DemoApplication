using System;
using Abstractions.Entitlement;

namespace DemoApplication.Models;

#nullable enable
public class EntitlementExceptionsModel : IEntitlementExceptions
{
    public Guid UserId { get; set; }
    public DateTimeOffset? ExpiresOn { get; set; } //nullable if null then does not expire

    public bool? ViewNotifications { get; set; }
    public bool? ViewUsers { get; set; }
    public bool? SuspendUsers { get; set; }
    public bool? StatisticalReport { get; set; }
    public bool? AuditReport { get; set; }
    public bool? AuthorizeMakerChecker { get; set; }
    public bool? EntitlementChange { get; set; }
    public bool? AuditLogs { get; set; }
}
