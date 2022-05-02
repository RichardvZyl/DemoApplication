using Abstractions.Results;
using DemoApplication.Entitlement;
using DemoApplication.Framework;
using DemoApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoApplication.Api.Controllers;

/// <summary> Audit Controller used to view Audit Trail </summary>
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
    /// <response code="200">Succesful response returns the last months audit trail entries</response>
    /// <response code="204">Succesful response but records exist</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response>
    /// <response code="403">Forbidden The client does not have access rights to the content</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    [ProducesResponseType(200, Type = typeof(IEnumerable<AuditTrailModel>))]
    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [HttpGet("LastMonth")]
    [MapToApiVersion("1")]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> GetLastMonth()
        => !User.HasClaim(x => x.Value == Claims.AuditLogs)
            ? await Task.FromResult((IActionResult)Forbid())
            : await _auditTrailService.ListLastMonthAsync().ResultAsync();

    /// <summary> Returns a list of all audit entries </summary>
    /// <returns>AuditTrailModel[]</returns>
    /// <remarks> Request all audit trail entries </remarks>
    /// <response code="200">Succesfull response return all the audit trail entries</response>
    /// <response code="204">Succesfull response but no records exist</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response>
    /// <response code="403">Forbidden The client does not have access rights to the content</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    [ProducesResponseType(200, Type = typeof(IEnumerable<AuditTrailModel>))]
    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [HttpGet]
    [MapToApiVersion("1")]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> GetAll()
        => !User.HasClaim(x => x.Value == Claims.AuditLogs)
            ? await Task.FromResult((IActionResult)Forbid())
            : await _auditTrailService.ListAsync().ResultAsync();
}
