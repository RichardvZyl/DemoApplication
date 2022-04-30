using System.Threading.Tasks;
using Abstractions.Results;
using DemoApplication.Framework;
using DemoApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Api.Controllers;

/// <summary> Authorization Controller used for login </summary>
[ApiController]
[Produces("application/json")]
[ApiVersion("1")]
[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class AuthsController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary> Auth Controller Constructor </summary>
    public AuthsController
    (
        IAuthService authService
    ) => _authService = authService;

    /// <summary> Sign In </summary>
    /// <param name="signInModel"></param>
    /// <returns>TokenModel</returns>
    /// <remarks> Sign in to the system to receive your bearer token </remarks>
    /// <response code="200">Succesful response returns an authorization token</response>
    /// <response code="204">Succesful response but user does not exist</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    [ProducesResponseType(200, Type = typeof(TokenModel))]
    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(IResult))]
    [AllowAnonymous]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(TokenModel))]
    [HttpPost]
    public async Task<IActionResult> SignInAsync(SignInModel signInModel)
        => await _authService.SignInAsync(signInModel).ResultAsync();

}


