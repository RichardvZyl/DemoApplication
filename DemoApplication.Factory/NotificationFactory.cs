using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Factory;

public static class NotificationFactory
{
    public static Notification Create(NotificationModel model)
        => new
        (
            model.Originator,
            model.Severity,
            model.Description,
            model.Date,
            model.Read,
            model.SeenBy,
            model.SeenAt,
            model.RelatedId,
            model.Entity,
            model.ForRole
        );
}
