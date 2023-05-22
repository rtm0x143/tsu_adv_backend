using System.Net;
using AdminPanel.Models;
using AdminPanel.PresentationServices;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers;

public class AdminPanelController : Controller
{
    [FromServices] public IEnumerable<IMvcErrorHandler> ErrorHandlers { get; init; } = default!;

    public virtual async Task<IActionResult> ErrorView(Exception? exception)
    {
        var context = new MvcErrorHandleContext(exception, HttpContext);
        foreach (var mvcErrorHandler in ErrorHandlers)
        {
            await mvcErrorHandler.Handle(context);
            if (context.IsCompleted) break;
        }

        if (context.TryGetResult(out var actionResult)) return actionResult;

        var model = new ErrorViewModel
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Message = exception?.Message
        };

        if (TempData["UrlReferrer"] is string path
            && Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var uri)) model.UrlReferrer = uri;

        return View("Error", model);
    }
}