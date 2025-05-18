using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Requests.Auth;

namespace NewspaperCreator.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authService.RegisterUserAsync(request, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] SignInRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authService.SignIn(request, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }
}
