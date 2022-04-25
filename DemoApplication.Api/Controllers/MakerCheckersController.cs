using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

/// <summary> Maker Checker Controller for Maker Checker interactions </summary>
[ApiVersion("1")]
[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
public sealed class MakerCheckersController : ControllerBase
{
    #region Private Members
    private readonly IMakerCheckerService _makerCheckerService;
    private readonly IUserService _userService;
    private readonly IEntitlementExceptionsService _entitlementExceptionsService;
    private readonly IAuthService _authService;
    #endregion

    #region Constructor
    /// <summary> MakerChecker constructor that grants access to the Voucher Service and the Maker Checker Service </summary>
    public MakerCheckersController
    (
        IMakerCheckerService makerCheckerService,
        IUserService userService,
        IEntitlementExceptionsService entitlementExceptionsService,
        IAuthService authService
    )
    {
        _makerCheckerService = makerCheckerService;
        _userService = userService;
        _entitlementExceptionsService = entitlementExceptionsService;
        _authService = authService;
    }
    #endregion

    #region Create

    /// <summary> Disable / Inactivate a user  </summary>
    /// <param name="makerChecker"></param>
    /// <returns>Guid</returns>
    /// <remarks> Request disabling a user by sending the user ID and receiving a result response </remarks>
    [HttpPost("SuspendUser")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IResult<Guid>))]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> SuspendUserAsync(NewMakerCheckerModel makerChecker)
    {
        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);

        if (!User.HasClaim(x => x.Value == Claims.SuspendUsers))
            return await Task.FromResult((IActionResult)Unauthorized());

        if (User.HasClaim(x => x.Value == Claims.AuthorizeMakerChecker))
        {//If a user can Authorize Maker Checker he can do the action directly
            var userToSuspend = JsonSerializer.Deserialize<Guid>(makerChecker.Model);
            return await _userService.SuspendAsync(userToSuspend, currentUserId).ResultAsync();
        }

        makerChecker.Action = MakerCheckerActionsEnum.SuspendUser;

        return await _makerCheckerService.AddAsync(makerChecker, currentUserId).ResultAsync();
    }

    /// <summary> Create a request to create a new User </summary>
    /// <param name="makerChecker"></param>
    /// <returns>Guid</returns>
    /// <remarks> Register a new user </remarks>
    [HttpPost("Register")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IResult<Guid>))]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync(NewMakerCheckerModel makerChecker)
    {
        var currentUserId = Guid.Empty; //Allow anonymous but to ensure that audits etc do not complain set this as the Guid

        makerChecker.Action = MakerCheckerActionsEnum.CreateUser;

        return await _makerCheckerService.AddAsync(makerChecker, currentUserId).ResultAsync();
    }

    /// <summary> Create a request to Change a users password </summary>
    /// <param name="makerChecker"></param>
    /// <returns>Result</returns>
    /// <remarks> Change a users password </remarks>
    [HttpPost("ChangePassword")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IResult))]
    [AllowAnonymous]
    public async Task<IActionResult> ChangePassword(NewMakerCheckerModel makerChecker)
    {
        var currentUserId = Guid.Empty; //Allow anonymous but to ensure that audits etc do not complain set this as the UserId

        makerChecker.Action = MakerCheckerActionsEnum.ChangePassword;

        return await _makerCheckerService.AddAsync(makerChecker, currentUserId).ResultAsync();
    }

    /// <summary> Create a request to Change a users Role/Entitlement </summary>
    /// <returns>Result</returns>
    /// <param name="makerChecker"></param>
    /// <remarks> Change User entitlement/role </remarks>
    [HttpPost("Entitlement")]
    [MapToApiVersion("1")]
    [DisableRequestSizeLimit]
    [ProducesResponseType(200, Type = typeof(IResult<Guid>))]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> EntitlementAsync(NewMakerCheckerModel makerChecker)
    {
        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);

        if (!User.HasClaim(x => x.Value == Claims.EntitlementChange))
            return await Task.FromResult((IActionResult)Unauthorized());

        if (User.HasClaim(x => x.Value == Claims.AuthorizeMakerChecker))
        {//If a user can Authorize Maker Checker he can do the action directly
            var roleChangeModel = JsonSerializer.Deserialize<RoleChangeModel>(makerChecker.Model);
            var result = _authService.ChangeRole(roleChangeModel ?? new(), currentUserId);

            return result.Result.Succeeded
                ? _entitlementExceptionsService.ModelHasExceptions(roleChangeModel?.ClaimExceptions ?? new())
                    ? await _entitlementExceptionsService.AddAsync(roleChangeModel?.ClaimExceptions ?? new(), roleChangeModel?.Login ?? string.Empty, currentUserId).ResultAsync()
                    : await _entitlementExceptionsService.DeleteAsync(roleChangeModel?.Login ?? string.Empty, currentUserId).ResultAsync()
                : await result.ResultAsync();
        }

        makerChecker.Action = MakerCheckerActionsEnum.Entitlement;

        return await _makerCheckerService.AddAsync(makerChecker, currentUserId).ResultAsync();
    }
    #endregion

    #region Read
    /// <summary> Get a specific Maker Checker Entry </summary>
    /// <returns>MakerCheckerModel</returns>
    /// <param name="id"></param>
    /// <remarks> Request a Maker Checker entry by supplying the GUID </remarks>
    [HttpGet("{id}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(MakerCheckerModel))]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> GetAsync(Guid id) 
        => !User.HasClaim(x => x.Value == Claims.AuthorizeMakerChecker)
            ? await Task.FromResult((IActionResult)Unauthorized())
            : await _makerCheckerService.GetByIdAsync(id).ResultAsync();

    /// <summary> Get a specific Maker Checker Entry files </summary>
    /// <returns>MakerCheckerModel</returns>
    /// <param name="id"></param>
    /// <remarks> Request a Maker Checker entry' files by supplying the GUID </remarks>
    [HttpGet("Documents/{id}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(MakerCheckerModel))]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> GetFilesAsync(Guid id) 
        => !User.HasClaim(x => x.Value == Claims.AuthorizeMakerChecker)
            ? await Task.FromResult((IActionResult)Unauthorized())
            : await _makerCheckerService.GetByIdAsync(id).ResultAsync();

    /// <summary> Get a list of all Maker Checkers </summary>
    /// <returns>MakerCheckerModel[]</returns>
    /// <remarks> Request a list of all Maker Checkers </remarks>
    [HttpGet]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<MakerCheckerModel>))]
    [Authorize(Roles = "Administrator,Financial,Supervisor")]
    public async Task<IActionResult> ListAsync() 
        => !User.HasClaim(x => x.Value == Claims.AuthorizeMakerChecker)
            ? await Task.FromResult((IActionResult)Unauthorized())
            : await _makerCheckerService.ListAsync().ResultAsync();
    #endregion

    #region Update
    /// <summary> Approve a Maker Checker Entry </summary>
    /// <returns>Result</returns>
    /// <param name="id"></param>
    /// <remarks> Approve a Maker Checker entry and action the original request </remarks>
    [HttpPost("Approve/{id}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IResult))]
    [Authorize(Roles = "Administrator,Supervisor")]
    public async Task<IActionResult> Approve(Guid id)
    {
        if (!User.HasClaim(x => x.Value == Claims.AuthorizeMakerChecker))
            return await Task.FromResult((IActionResult)Unauthorized());

        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);

        return await _makerCheckerService.ApproveAsync(id, currentUserId).ResultAsync();
    }

    /// <summary> Deny a Maker Checker Entry </summary>
    /// <returns>Result</returns>
    /// <param name="id"></param>
    /// <remarks> Deny a Maker Checker Entry and take no further action </remarks>
    [HttpPost("Deny/{id}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IResult))]
    [Authorize(Roles = "Administrator,Supervisor")]
    public async Task<IActionResult> Deny(Guid id)
    {
        if (!User.HasClaim(x => x.Value == Claims.AuthorizeMakerChecker))
            return await Task.FromResult((IActionResult)Unauthorized());

        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);

        return await _makerCheckerService.DenyAsync(id, currentUserId).ResultAsync();
    }
    #endregion
}
