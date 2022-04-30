using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.AspNetCore;
using Abstractions.Results;
using DemoApplication.Entitlement;
using DemoApplication.Enums;
using DemoApplication.Factory;
using DemoApplication.Framework;
using DemoApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoApplication.Api.Controllers;

/// <summary> Entitlement Controller used for retrieving default entitlement </summary>
[ApiController]
[Produces("application/json")]
[ApiVersion("1")]
[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class EntitlementController : ControllerBase
{
    private readonly IEntitlementExceptionsService _entitlementExceptionsService;
    private readonly IEntitlementService _entitlementService;

    /// <summary> Entitlment Exceptions Controller Constructor grants access to the IndustryType service </summary>
    public EntitlementController
    (
        IEntitlementExceptionsService entitlementExceptionsService,
        IEntitlementService entitlementService
    )
    {
        _entitlementExceptionsService = entitlementExceptionsService;
        _entitlementService = entitlementService;
    }

    /// <summary> Get Default Entitlement for given role </summary>
    /// <param name="role"></param>
    /// <returns>EntilementExceptionsModel</returns>
    /// <remarks> Get the Default Entitlement for the given role </remarks>
    /// <response code="200">Entitlement returned for provided parameter</response>
    /// <response code="204">No Entitlement found for provided parameter</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response>
    /// <response code="403">Forbidden The client does not have access rights to the content</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    [ProducesResponseType(200, Type = typeof(EntitlementModel))]
    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(403, Type = typeof(ForbidResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [HttpGet("Role/{role}")]
    [MapToApiVersion("1")]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> GetDefaultEntitlement(RolesEnum role) 
        => //if the user cannot change entitlement he does not need to see the entitlement
        !User.HasClaim(x => x.Value == Claims.EntitlementChange)
            ? await Task.FromResult((IActionResult)Forbid())
            : await _entitlementService.GetDefaultEntitlement(role).ResultAsync();

    /// <summary> Get Current User Entitlement </summary>
    /// <param name="email"></param>
    /// <returns>EntitlementExceptionsModel</returns>
    /// <remarks> Get the given user entitlement exceptions </remarks>
    /// <response code="200">Entitlement returned for provided parameter</response>
    /// <response code="204">No Entitlement found for provided parameter</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response>
    /// <response code="403">Forbidden The client does not have access rights to the content</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    [ProducesResponseType(200, Type = typeof(EntitlementModel))]
    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(403, Type = typeof(ForbidResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [HttpGet("User/{email}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(EntitlementExceptionsModel))]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> GetUserEntitlement(string email)
    {
        //if the user cannot change entitlement he does not need to see the entitlement
        if (!User.HasClaim(x => x.Value == Claims.EntitlementChange))
        {
            return await Task.FromResult((IActionResult)Forbid());
        }
        else
        {
            var entitlement = await _entitlementService.GetByUserEmailAsync(email);
            var entitlementExceptions = await _entitlementExceptionsService.GetByUserEmailAsync(email);

            return await Task.FromResult(Ok(EntitlementFactory.ApplyExceptionsToEntitlement(entitlement, entitlementExceptions)));
        }
    }

    /// <summary> Get Current User Role and return it as a value pair string (for dropdown use) </summary>
    /// <param name="email"></param>
    /// <returns>string</returns>
    /// <remarks> Get the given user entitlement exceptions </remarks>
    /// <response code="200">Role returned for provided user email</response>
    /// <response code="204">No Role (user) found for provided email</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response>
    /// <response code="403">Forbidden The client does not have access rights to the content</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    [ProducesResponseType(200, Type = typeof(RolesEnum))]
    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(403, Type = typeof(ForbidResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [HttpGet("User/Role/{email}")]
    [MapToApiVersion("1")]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> GetUserRole(string email) 
        => //if the user cannot change entitlement he does not need to see the entitlement
        !User.HasClaim(x => x.Value == Claims.EntitlementChange)
            ? await Task.FromResult((IActionResult)Forbid())
            : await _entitlementExceptionsService.GetRoleByUserEmailAsync(email).ResultAsync();
}
