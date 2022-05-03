using Abstractions.EntityFrameworkCore;
using DemoApplication.Domain;
using DemoApplication.Enums;
using DemoApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Database;

public sealed class NotificationRepository : EFRepository<Notification>, INotificationRepository
{
    public NotificationRepository(Context context) : base(context) { }


    public async Task<NotificationModel> GetByIdAsync(Guid id)
        => await Queryable.Where(NotificationExpression.Id(id)!).Select(NotificationExpression.Model!).SingleOrDefaultAsync() ?? new();

    public Task ReadAsync(Notification notification, Guid user)
        => UpdatePartialAsync(notification.Id, new { notification.Read, notification.SeenAt, notification.SeenBy });

    public async Task<IEnumerable<NotificationModel>> GetByRoleAsync(RolesEnum role)
        => await Queryable.Where(NotificationExpression.OneMonth()!).Where(NotificationExpression.ForRole(role)!).Select(NotificationExpression.Model!).ToListAsync();

    public async Task<IEnumerable<NotificationModel>> GetAlertsOnly()
        => await Queryable.Where(NotificationExpression.OneMonth()!).Where(NotificationExpression.SeriousAlerts()!).Select(NotificationExpression.Model!).ToListAsync();

    public async Task<IEnumerable<NotificationModel>> GetAlertsForRole(RolesEnum role)
        => await Queryable.Where(NotificationExpression.OneMonth()!).Where(NotificationExpression.SeriousAlerts()!).Where(NotificationExpression.ForRole(role)!).Select(NotificationExpression.Model!).ToListAsync();

}
