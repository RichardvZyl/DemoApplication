using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    /// <response code="200">Succesfull result returns user with provided ID</response>
    /// <response code="204">No user found with provided ID</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response>
    /// <response code="403">Forbidden The client does not have access rights to the content</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    [ProducesResponseType(200, Type = typeof(UserModel))]
    [ProducesResponseType(204, Type = typeof(NotFoundResult))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(403, Type = typeof(ForbidResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [HttpGet("{id}")]
    [MapToApiVersion("1")]
    [Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        if (!User.HasClaim(x => x.Value == Claims.ViewUsers))
            return await Task.FromResult((IActionResult)Forbid());

        var currentUserRole = Enum.Parse<RolesEnum>(User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value!);

        return await _userService.GetAsync(id, currentUserRole).ResultAsync();
    }

    /// <summary> Retrieve the currently logged in user </summary>
    /// <returns>UserModel</returns>
    /// <remarks> Request user details of the currently signd in user, receiving the user model as a response </remarks>
    /// <response code="200">Succesfull result returns current user</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    [ProducesResponseType(200, Type = typeof(UserModel))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [HttpGet("Current")]
    [MapToApiVersion("1")]
    [Authorize]
    public async Task<IActionResult> GetCurrentAsync()
    {
        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);
        var currentUserRole = Enum.Parse<RolesEnum>(User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value!);

        return await _userService.GetAsync(currentUserId, currentUserRole).ResultAsync();
    }

    /// <summary> Get a list of all user </summary>
    /// <returns>UserModel[]</returns>
    /// <remarks> Request a list of all saved users </remarks>
    /// <response code="200">Succesfull result returns all users</response>
    /// <response code="204">No users found</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response>
    /// <response code="403">Forbidden The client does not have access rights to the content</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserModel>))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(204, Type = typeof(NotFoundResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(403, Type = typeof(ForbidResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [HttpGet]
    [MapToApiVersion("1")]
    [Authorize(Roles = "Administrator,Financial,Supervisor,Clerk")]
    public async Task<IActionResult> ListAsync()
    {
        if (!User.HasClaim(x => x.Value == Claims.ViewUsers))
            return await Task.FromResult((IActionResult)Forbid());

        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);
        var currentUserRole = Enum.Parse<RolesEnum>(User.Claims.FirstOrDefault(c => c.Type == "Role")?.Value!);

        return await _userService.ListAsync(currentUserRole).ResultAsync();
    }

    /// <summary> Update user details </summary>
    /// <returns>Result</returns>
    /// <param name="userId"></param>
    /// <param name="patchDoc"></param>
    /// <remarks> Request updating a user by providing the new user details and receiving a result </remarks>
    /// <response code="200">Succesfull result returns Ok result</response>
    /// <response code="204">No user found with provided ID</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response>
    /// <response code="403">Forbidden The client does not have access rights to the content</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    [ProducesResponseType(200, Type = typeof(OkResult))]
    [ProducesResponseType(204, Type = typeof(NotFoundResult))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(403, Type = typeof(ForbidResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [HttpPatch("{userId}")]
    [MapToApiVersion("1")]
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

            var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);

            return userModel.Id != currentUserId
                ? Forbid()
                : await _userService.UpdateAsync(userModel).ResultAsync();
        }

        return BadRequest(ModelState);
    }
}
