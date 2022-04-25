using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.AspNetCore;
using Abstractions.Results;
using DemoApplication.Entitlement;
using DemoApplication.Enums;
using DemoApplication.Framework;
using DemoApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoApplication.Api.Controllers;

/// <summary> Notification controller for notification interactions </summary>
[ApiController]
[ApiVersion("1")]
[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
public sealed class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    /// <summary> Notification Constructor provides access to the notification service </summary>
    public NotificationsController(INotificationService notificationService)
        => _notificationService = notificationService;

    /// <summary> Get a list of all notifications </summary>
    /// <returns>NotficationModel</returns>
    /// <remarks> Request a list of all notifications </remarks>
    [HttpGet]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<NotificationModel>))]
    [Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
    public async Task<IActionResult> ListAsync() 
        => !User.HasClaim(x => x.Value == Claims.ViewNotifications)
            ? await Task.FromResult((IActionResult)Unauthorized())
            : await _notificationService.ListAsync().ResultAsync();

    /// <summary> Get a list of all notifications by current Role </summary>
    /// <returns>NotficationModel</returns>
    /// <remarks> Request a list of all notifications By Role (Role is determined by logged in user) </remarks>
    [HttpGet("ByRole")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<NotificationModel>))]
    [Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
    public async Task<IActionResult> ListByRoleAsync()
    {
        if (!User.HasClaim(x => x.Value == Claims.ViewNotifications))
            return await Task.FromResult((IActionResult)Unauthorized());

        RolesEnum currentUserRole = Enum.Parse<RolesEnum>(User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value!);

        return await _notificationService.ListByRole(currentUserRole).ResultAsync();
    }

    /// <summary> Get all notifications with Alert status (Serious) </summary>
    /// <returns>NotficationModel</returns>
    /// <remarks> Request notifications with a serious/error status </remarks>
    [HttpGet("Alerts")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<NotificationModel>))]
    [Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
    public async Task<IActionResult> ListAlertsAsync() 
        => !User.HasClaim(x => x.Value == Claims.ViewNotifications)
            ? await Task.FromResult((IActionResult)Unauthorized())
            : await _notificationService.ListAlertsOnlyAsync().ResultAsync();

    /// <summary> Get a specific notification based on received id </summary>
    /// <param name="id"></param>
    /// <returns>NotficationModel</returns>
    /// <remarks> Get notification details by supplying the notification ID </remarks>
    [HttpGet("{id}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(NotificationModel))]
    [Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
    public async Task<IActionResult> GetAsync(Guid id) 
        => !User.HasClaim(x => x.Value == Claims.ViewNotifications)
            ? await Task.FromResult((IActionResult)Unauthorized())
            : await _notificationService.GetAsync(id).ResultAsync();

    /// <summary> Mark a Notification as read </summary>
    /// <param name="id"></param>
    /// <returns>Result</returns>
    /// <remarks> Read a notification by supplying it's ID and the current user email </remarks>
    [HttpPost("{id}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IResult))]
    [Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
    public async Task<IActionResult> ReadNotification(Guid id)
    {
        if (!User.HasClaim(x => x.Value == Claims.ViewNotifications))
            return await Task.FromResult((IActionResult)Unauthorized());

        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);

        return await _notificationService.ReadAsync(id, currentUserId).ResultAsync();
    }
}
