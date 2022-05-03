using System.Linq.Expressions;
using DemoApplication.Domain;
using DemoApplication.Enums;
using DemoApplication.Models;

namespace DemoApplication.Database;

public static class NotificationExpression
{
    #region Values
    public static Expression<Func<Notification, Guid>> Id() => notification => notification.Id;
    public static Expression<Func<Notification, Guid>> Originator() => notification => notification.Originator;
    public static Expression<Func<Notification, string>> Description() => notification => notification.Description;
    public static Expression<Func<Notification, DateTimeOffset>> Date() => notification => notification.Date;
    public static Expression<Func<Notification, SeverityEnum>> Severity() => notification => notification.Severity;
    public static Expression<Func<Notification, bool>> Read() => notification => notification.Read;
    public static Expression<Func<Notification, Guid>> SeenBy() => notification => notification.SeenBy;
    public static Expression<Func<Notification, DateTimeOffset?>> SeenAt() => notification => notification.SeenAt;
    public static Expression<Func<Notification, EntityEnum>> Entity() => notification => notification.Entity;
    public static Expression<Func<Notification, string>> RelatedId() => notification => notification.RelatedId;
    public static Expression<Func<Notification, RolesEnum>> ForRole() => notification => notification.ForRole;
    public static Expression<Func<Notification, NotificationModel>> Model => notification => new NotificationModel
    {
        Id = notification.Id,
        Originator = notification.Originator,
        Description = notification.Description,
        Date = notification.Date,
        Severity = notification.Severity,
        Read = notification.Read,
        SeenBy = notification.SeenBy,
        SeenAt = notification.SeenAt,
        Entity = notification.Entity,
        RelatedId = notification.RelatedId,
        ForRole = notification.ForRole
    };
    #endregion

    #region Get By Values
    public static Expression<Func<Notification, bool>> Read(bool read) => notification => notification.Read == read;
    public static Expression<Func<Notification, bool>> Severity(SeverityEnum severity) => notification => notification.Severity == severity;
    public static Expression<Func<Notification, bool>> Originator(Guid id) => notification => notification.Originator == id;
    public static Expression<Func<Notification, bool>> Entity(EntityEnum entity) => notification => notification.Entity == entity;
    public static Expression<Func<Notification, bool>> ForRole(RolesEnum role) => notification => notification.ForRole == role;
    public static Expression<Func<Notification, bool>> Id(Guid id) => notification => notification.Id == id;
    public static Expression<Func<Notification, bool>> SeriousAlerts() => notifications => (int)notifications.Severity > (int)SeverityEnum.General;
    public static Expression<Func<Notification, bool>> OneMonth() => notifications => DateTimeOffset.UtcNow.AddMonths(-1).CompareTo(notifications.Date) < 0;
    #endregion 
}
