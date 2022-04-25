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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Api.Controllers;

/// <summary> User controller for user interactions </summary>
[ApiController]
[ApiVersion("1")]
[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[AllowAnonymous]
//[Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    /// <summary> User controller constructor </summary>
    public UsersController(IUserService userService)
        => _userService = userService;

    /// <summary> Retrieve an existing user </summary>
    /// <returns>UserModel</returns>
    /// <param name="id"></param>
    /// <remarks> Request user details by providing the user id and receiving the user model as a response </remarks>
    [HttpGet("{id}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(UserModel))]
    [Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        if (!User.HasClaim(x => x.Value == Claims.ViewUsers))
            return await Task.FromResult((IActionResult)Unauthorized());

        var currentUserRole = Enum.Parse<RolesEnum>(User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value!);

        return await _userService.GetAsync(id, currentUserRole).ResultAsync();
    }

    /// <summary> Retrieve the currently logged in user </summary>
    /// <returns>UserModel</returns>
    /// <remarks> Request user details of the currently signd in user, receiving the user model as a response </remarks>
    [HttpGet("Current")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(UserModel))]
    [Authorize]
    public async Task<IActionResult> GetCurrentAsync()
    {
        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);
        //RolesEnum currentUserRole = Enum.Parse<RolesEnum>(User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value!);

        return await _userService.GetAsync(currentUserId, RolesEnum.Administrator).ResultAsync();
    }

    /// <summary> Get a list of all user </summary>
    /// <returns>UserModel[]</returns>
    /// <remarks> Request a list of all saved users </remarks>
    [HttpGet]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserModel>))]
    //[Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
    [AllowAnonymous]
    public async Task<IActionResult> ListAsync()
    {
        ////if (!User.HasClaim(x => x.Value == Claims.ViewUsers))
        ////    return await Task.FromResult((IActionResult)Unauthorized());

        //Guid currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);
        //var currentUserRole = Enum.Parse<RolesEnum>(User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value!);
        var currentUserRole = RolesEnum.Administrator;

        return await _userService.ListAsync(currentUserRole).ResultAsync();
    }

    /// <summary> Update user details </summary>
    /// <returns>Result</returns>
    /// <param name="userId"></param>
    /// <param name="patchDoc"></param>
    /// <remarks> Request updating a user by providing the new user details and receiving a result </remarks>
    [HttpPatch("{userId}")]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(IResult))]
    [Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
    public async Task<IActionResult> UpdateAsync(Guid userId, [FromBody] JsonPatchDocument<UserModel> patchDoc)
    {
        if (patchDoc != null)
        {
            var userModel = await _userService.GetAsync(userId, RolesEnum.Administrator);

            if (userModel == null)
                return BadRequest(ModelState);

            patchDoc.ApplyTo(userModel);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Consider a check to see if user being updated is user logged in
            return await _userService.UpdateAsync(userModel).ResultAsync();
        }

        return BadRequest(ModelState);
    }
}
