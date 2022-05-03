using System.Linq.Expressions;
using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Database.AuditTrails;

public static class AuditTrailExpression
{
    #region Values
    public static Expression<Func<AuditTrail, Guid>> Id() => auditTrail => auditTrail.Id;
    public static Expression<Func<AuditTrail, Guid>> UserId() => auditTrail => auditTrail.UserId;
    public static Expression<Func<AuditTrail, DateTimeOffset>> Date() => auditTrail => auditTrail.Date;
    public static Expression<Func<AuditTrail, string>> Contents() => auditTrail => auditTrail.Contents;
    public static Expression<Func<AuditTrail, string>> JsonModel() => jsonModel => jsonModel.Model;
    public static Expression<Func<AuditTrail, string>> DisplayContext() => auditTrail => auditTrail.DisplayContext;
    public static Expression<Func<AuditTrail, AuditTrailModel>> Model => auditTrail => new AuditTrailModel
    {
        Id = auditTrail.Id,
        Date = auditTrail.Date,
        UserId = auditTrail.UserId,
        DisplayContext = auditTrail.DisplayContext,
        Model = auditTrail.Model,
        Contents = auditTrail.Contents
    };
    #endregion

    #region GetByValues
    public static Expression<Func<AuditTrail, bool>> OneMonth() => auditTrail => auditTrail.Date > DateTimeOffset.UtcNow.AddMonths(-1);
    public static Expression<Func<AuditTrail, bool>> Id(Guid id) => auditTrail => auditTrail.Id == id;
    public static Expression<Func<AuditTrail, bool>> UserId(Guid id) => auditTrail => auditTrail.UserId == id;
    public static Expression<Func<AuditTrail, bool>> LastMonth() => auditTrail => DateTimeOffset.Compare(auditTrail.Date, DateTimeOffset.UtcNow.AddMonths(-1)) > 0;
    #endregion
}
