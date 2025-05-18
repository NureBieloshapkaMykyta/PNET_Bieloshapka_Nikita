using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Requests.Auth;

namespace NewspaperCreator.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await _authService.RegisterUserAsync(request, cancellationToken);

        if (!result.IsSuccessful)
        {
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(SignInRequest request, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        var result = await _authService.SignIn(request, cancellationToken);

        if (!result.IsSuccessful)
        {
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [Route("api/[controller]/register")]
    public async Task<IActionResult> RegisterApi([FromBody] RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _authService.RegisterUserAsync(request, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok();
    }

    [HttpPost]
    [Route("api/[controller]/login")]
    public async Task<IActionResult> LoginApi([FromBody] SignInRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _authService.SignIn(request, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }
}
