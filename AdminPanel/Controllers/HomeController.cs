using AdminPanel.Models;
using AdminPanel.Services;
using Common.App.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;

namespace AdminPanel.Controllers;

public class HomeController : AdminPanelController
{
    public IActionResult Index()
    {
        return View();
    }

    // [Authorize]
    public async Task<IActionResult> Profile([FromServices] IProfileRepository profileRepository)
    {
        var selfProfileResult = await profileRepository.GetSelfProfile();
        return selfProfileResult.Succeeded()
            ? View(selfProfileResult.Value())
            : await ErrorView(selfProfileResult.Error());
    }

    public Task<IActionResult> Error()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        return ErrorView(exceptionHandlerPathFeature?.Error);
    }
}