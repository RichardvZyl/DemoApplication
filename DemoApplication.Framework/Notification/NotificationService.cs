using Abstractions.Results;
using DemoApplication.Database;
using DemoApplication.Domain;
using DemoApplication.Enums;
using DemoApplication.Factory;
using DemoApplication.Models;
using DemoApplication.Validator;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Framework;

public sealed class NotificationService : INotificationService
{
    #region Private Members
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationRepository _notficationRepository;
    private readonly IAuditTrailService _auditTrailService;
    #endregion

    #region Constructor
    public NotificationService
    (
        IUnitOfWork unitOfWork,
        INotificationRepository notificationRepository,
        IAuditTrailService auditTrailService
    )
    {
        _unitOfWork = unitOfWork;
        _notficationRepository = notificationRepository;
        _auditTrailService = auditTrailService;
    }
    #endregion


    #region Create
    public async Task<IResult<Guid>> AddAsync(Guid activeUserId, SeverityEnum severity, string description, RolesEnum forRole, EntityEnum entity, string relatedId)
    {//This should not be logged as only the system can add notifications
        var model = new NotificationModel
        {
            Date = DateTimeOffset.UtcNow,
            Originator = activeUserId,
            Severity = severity,
            Description = description,
            Read = false,
            SeenBy = Guid.Empty,
            SeenAt = new DateTimeOffset?(),
            ForRole = forRole,
            Entity = entity,
            RelatedId = relatedId
        };

        var validation = await new AddNotificationModelValidator().ValidateAsync(model);

        if (validation.Failed)
            return Result<Guid>.Fail(validation.Message);

        var notification = NotificationFactory.Create(model);

        await _notficationRepository.AddAsync(notification);

        await _unitOfWork.SaveChangesAsync();

        return await Result<Guid>.SuccessAsync(notification.Id);
    }
    #endregion

    #region Read
    public async Task<NotificationModel> GetAsync(Guid id) => await _notficationRepository.GetByIdAsync(id);

    public async Task<IEnumerable<NotificationModel>> ListAsync() => await _notficationRepository.Queryable.Where(NotificationExpression.OneMonth()!).Select(NotificationExpression.Model!).ToListAsync();
    public async Task<IEnumerable<NotificationModel>> ListAlertsOnlyAsync() => await _notficationRepository.GetAlertsOnly();
    public async Task<IEnumerable<NotificationModel>> ListByRole(RolesEnum role) => await _notficationRepository.GetByRoleAsync(role);
    public async Task<IEnumerable<NotificationModel>> ListAlertsByRole(RolesEnum role) => await _notficationRepository.GetAlertsForRole(role);
    #endregion

    #region Update
    public async Task<IResult> ReadAsync(Guid id, Guid activeUserId)
    {
        var notification = new Notification(id);

        var auditResult = await _auditTrailService.AddAsync(notification, activeUserId);

        if (auditResult.Failed)
            return await Result.FailAsync(auditResult.Message);

        notification.ReadNotification(activeUserId);

        await _notficationRepository.ReadAsync(notification, activeUserId);

        await _unitOfWork.SaveChangesAsync();

        return await Result.SuccessAsync();
    }
    #endregion
}

