using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.PresentationServices;

public class MvcErrorHandleContext
{
    public IActionResult? Result { get; private set; }
    public bool IsCompleted => Result != null;

    public bool TryGetResult([NotNullWhen(true)] out IActionResult? result)
    {
        result = Result;
        return IsCompleted;
    }

    public MvcErrorHandleContext(object? error, HttpContext httpContext, string? urlReferrer)
    {
        Error = error;
        HttpContext = httpContext;
        UrlReferrer = urlReferrer;
    }

    public MvcErrorHandleContext(object? error, HttpContext httpContext)
        : this(error, httpContext, httpContext.Request.Path.Value)
    {
    }

    /// <summary>
    /// object representing Error
    /// </summary>
    public object? Error { get; }

    /// <summary>
    /// Url where exception occured
    /// </summary>
    public string? UrlReferrer { get; }

    public HttpContext HttpContext { get; }

    public void Complete(IActionResult actionResult) => Result = actionResult;
}