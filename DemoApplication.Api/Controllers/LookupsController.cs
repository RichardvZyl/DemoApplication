using Abstractions.Extensions;
using DemoApplication.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoApplication.Api.Controllers;

/// <summary> Lookups controller for sharing all enums and possible values </summary>
[Authorize]
[ApiVersion("1")]
[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public sealed class LookupsController : ControllerBase
{
    /// <summary> Lookups controller constructor that grants access to roles, entities, makerCheckerActions, notificationSeverity </summary>
    public LookupsController() { }

    /// <summary>  Get a list of all roles </summary>
    /// <returns>RolesEnum[]</returns>
    /// <remarks> Request a list of all Roles </remarks>
    [HttpGet("RolesEnum")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RolesEnum>))]
    public async Task<IActionResult> Roles()
        => await Task.FromResult((IActionResult)Ok(EnumExtensions.GetEnumList<RolesEnum>()));

    /// <summary> Get a list of all Enities used for notifications </summary>
    /// <returns>EntityEnum[]</returns>
    /// <remarks> Request a list of all entities used in notifications </remarks>
    [HttpGet("Entity")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<EntityEnum>))]
    public async Task<IActionResult> Entity()
        => await Task.FromResult((IActionResult)Ok(EnumExtensions.GetEnumList<EntityEnum>()));

    /// <summary> Get a list of all Actions for Maker Checker </summary>
    /// <returns>MakerCheckerActionsEnum[]</returns>
    /// <remarks> Request a list of all Maker Checker actions </remarks>
    [HttpGet("MakerChecker")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MakerCheckerActionsEnum>))]
    public async Task<IActionResult> MakerChecker()
        => await Task.FromResult((IActionResult)Ok(EnumExtensions.GetEnumList<MakerCheckerActionsEnum>()));

    /// <summary> Get a list of Severity used in notifications </summary>
    /// <returns>SeverityEnum[]</returns>
    /// <remarks> Request a list Severity used for Notifications  </remarks>
    [HttpGet("NotificationSeverity")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<SeverityEnum>))]
    public async Task<IActionResult> Severity()
        => await Task.FromResult((IActionResult)Ok(EnumExtensions.GetEnumList<SeverityEnum>()));

    /// <summary> List Patch operation types </summary>
    /// <returns>OperationTypes[]</returns>
    /// <remarks> Request a list of the operation types used in the patch api calls </remarks>
    [HttpGet("OperationTypes")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<OperationTypes>))]
    public async Task<IActionResult> ListOperationTypesAsync()
        => await Task.FromResult((IActionResult)Ok(EnumExtensions.GetEnumList<OperationTypes>()));



}
