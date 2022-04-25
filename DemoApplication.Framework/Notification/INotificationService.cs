using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Results;
using DemoApplication.Enums;
using DemoApplication.Models;

namespace DemoApplication.Framework;

public interface INotificationService
{
    Task<IResult<Guid>> AddAsync(Guid activeUserId, SeverityEnum severity, string description, RolesEnum forRole, EntityEnum entity, string relatedId);

    Task<NotificationModel> GetAsync(Guid id);
    Task<IEnumerable<NotificationModel>> ListAsync();
    Task<IEnumerable<NotificationModel>> ListAlertsOnlyAsync();
    Task<IEnumerable<NotificationModel>> ListByRole(RolesEnum role);
    Task<IEnumerable<NotificationModel>> ListAlertsByRole(RolesEnum role);

    Task<IResult> ReadAsync(Guid id, Guid activeUserId);
}
