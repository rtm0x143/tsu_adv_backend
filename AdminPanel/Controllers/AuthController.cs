using AdminPanel.Infra.Http.Configuration;
using AdminPanel.Models;
using AdminPanel.Services;
using Common.App.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdminPanel.Controllers;

public class AuthController : AdminPanelController
{
    public IActionResult Index([FromQuery] string? errorMessage)
    {
        ViewData["ErrorMessage"] = errorMessage;
        return View();
    }

    public async Task<IActionResult> SignIn(SignInViewModel model,
        [FromServices] IAuthService authService,
        [FromServices] IOptions<CookieParametersOptions> options)
    {
        // test
        var result = await authService.Login(model.Email, model.Password);
        if (!result.Succeeded())
            return RedirectToAction("Index", routeValues: new { errorMessage = "Authentication failed" });

        var (accessToken, refreshToken) = result.Value();
        Response.Cookies.Append(options.Value.AccessTokenParameterName, accessToken);
        Response.Cookies.Append(options.Value.RefreshTokenParameterName, refreshToken);

        return RedirectToAction("Profile", "Home");
    }
}