using System.Net;
using AdminPanel.Infra.Http.Configuration;
using AdminPanel.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdminPanel.PresentationServices;

public class UnauthorizedMvcErrorHandler : IMvcErrorHandler
{
    private readonly IAuthService _authService;
    private readonly CookieParametersOptions _cookieParams;

    public UnauthorizedMvcErrorHandler(IOptions<CookieParametersOptions> options, IAuthService authService)
    {
        _authService = authService;
        _cookieParams = options.Value;
    }

    public async ValueTask Handle(MvcErrorHandleContext context)
    {
        if (context.Error is not HttpRequestException { StatusCode: HttpStatusCode.Unauthorized }) return;
        var refreshToken = context.HttpContext.Request.Cookies[_cookieParams.RefreshTokenParameterName];

        if (refreshToken == null
            || await _authService.Refresh(refreshToken) is not { IsT0: true } refreshResult)
        {
            context.Complete(new RedirectToActionResult("Index", "Auth",
                new { errorMessage = "Sign in to perform action" }));
            return;
        }

        var (accessToken, newRefreshToken) = refreshResult.AsT0;
        context.HttpContext.Response.Cookies.Delete(_cookieParams.AccessTokenParameterName);
        context.HttpContext.Response.Cookies.Append(_cookieParams.AccessTokenParameterName, accessToken);
        context.HttpContext.Response.Cookies.Delete(_cookieParams.RefreshTokenParameterName);
        context.HttpContext.Response.Cookies.Append(_cookieParams.RefreshTokenParameterName, newRefreshToken);

        context.Complete(new RedirectResult(context.UrlReferrer ?? "/Home"));
    }
}