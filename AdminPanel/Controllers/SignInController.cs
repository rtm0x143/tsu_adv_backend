using AdminPanel.Infra.Http;
using AdminPanel.Models;
using AdminPanel.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers;

public class SignInController : Controller
{
    private readonly IAuthService _authService;

    public SignInController(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Index() => View();

    public async Task<IActionResult> SignIn(SignInViewModel model)
    {
        var (accessToken, refreshToken) = await _authService.Login(model.Email, model.Password);

        // TODO

        return Ok();
    }
}