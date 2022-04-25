using System.Threading.Tasks;
using Abstractions.AspNetCore;
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
    [AllowAnonymous]
    [MapToApiVersion("1")]
    [ProducesResponseType(200, Type = typeof(TokenModel))]
    [HttpPost]
    public async Task<IActionResult> SignInAsync(SignInModel signInModel) 
        => await _authService.SignInAsync(signInModel).ResultAsync();

}


