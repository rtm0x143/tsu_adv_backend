using AdminPanel.Services;
using AdminPanel.ViewModels;
using Common.App.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace AdminPanel.Controllers;

public class HomeController : AdminPanelController
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Profile([FromServices] IProfileRepository profileRepository)
    {
        var selfProfileResult = await profileRepository.GetSelfProfile();
        return selfProfileResult.Succeeded()
            ? View(new ProfileViewModel { UserProfile = selfProfileResult.Value() })
            : await ErrorView(selfProfileResult.Error());
    }

    public Task<IActionResult> Error()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        return ErrorView(exceptionHandlerPathFeature?.Error);
    }

    public IActionResult Retry() => View();
}