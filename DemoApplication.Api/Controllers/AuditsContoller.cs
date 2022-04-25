using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.AspNetCore;
using DemoApplication.Entitlement;
using DemoApplication.Framework;
using DemoApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoApplication.Api.Controllers;

/// <summary> Accounting Controller used to view accounting data </summary>
[ApiVersion("1")]
[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(Roles = "Administrator,Financial,Supervisor")]
public sealed class AuditsController : ControllerBase  
{
    private readonly IAuditTrailService _auditTrailService;

    /// <summary> Accounting Controller Constructor grants access to the accounting service and the accounting history service </summary>
    public AuditsController
    (
        IAuditTrailService auditTrailService
    ) => _auditTrailService = auditTrailService;

    /// <summary> Returns a list of all audit entries in the last month </summary>
    /// <returns>AuditTrailModel[]</returns>
    /// <remarks> Request all audit trail entries of the last month </remarks>
    [HttpGet("LastMonth")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<AuditTrailModel>))]
    [MapToApiVersion("1")]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> GetLastMonth()
        => !User.HasClaim(x => x.Value == Claims.AuditLogs)
            ? await Task.FromResult((IActionResult)Unauthorized())
            : await _auditTrailService.ListLastMonthAsync().ResultAsync();

    /// <summary> Returns a list of all audit entries </summary>
    /// <returns>AuditTrailModel[]</returns>
    /// <remarks> Request all audit trail entries </remarks>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<AuditTrailModel>))]
    [MapToApiVersion("1")]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> GetAll()
        => !User.HasClaim(x => x.Value == Claims.AuditLogs)
            ? await Task.FromResult((IActionResult)Unauthorized())
            : await _auditTrailService.ListAsync().ResultAsync();
}
