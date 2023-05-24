using AdminPanel.PresentationServices;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers;

public class AdminPanelController : Controller
{
    [FromServices] public IEnumerable<IMvcErrorHandler> ErrorHandlers { get; init; } = default!;

    public virtual async Task<IActionResult> ErrorView(Exception? exception)
    {
        var context = new MvcErrorHandleContext(exception, HttpContext, null);
        foreach (var mvcErrorHandler in ErrorHandlers.Order())
        {
            await mvcErrorHandler.Handle(context);
            if (context.IsCompleted) break;
        }

        if (context.TryGetResult(out var actionResult)) return actionResult;

        context.ViewModel.Message ??= exception?.Message;

        if (context.ViewModel.UrlReferrer == null
            && TempData["UrlReferrer"] is string path
            && Uri.TryCreate(path, UriKind.RelativeOrAbsolute, out var uri)) context.ViewModel.UrlReferrer = uri;

        return View("Error", context.ViewModel);
    }
}