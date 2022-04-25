using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Repositories;
using DemoApplication.Domain;
using DemoApplication.Enums;
using DemoApplication.Models;

namespace DemoApplication.Database;

public interface INotificationRepository : IRepository<Notification>
{
    Task<NotificationModel> GetByIdAsync(Guid id);
    Task<IEnumerable<NotificationModel>> GetByRoleAsync(RolesEnum role);

    Task ReadAsync(Notification notification, Guid user);

    Task<IEnumerable<NotificationModel>> GetAlertsOnly();
    Task<IEnumerable<NotificationModel>> GetAlertsForRole(RolesEnum role);

}
