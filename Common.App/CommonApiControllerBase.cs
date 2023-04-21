using System.Security.Claims;
using Common.App.Attributes;
using Common.App.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.App;

[ApiController]
[VersioningApiRoute]
public abstract class CommonApiControllerBase<TConcreteController> : ControllerBase
    where TConcreteController : CommonApiControllerBase<TConcreteController>
{
    [FromServices] public IExceptionsDescriber ExceptionsDescriber { get; init; } = default!;
    [FromServices] public IAuthorizationService AuthService { get; init; } = default!;
    [FromServices] public ILogger<TConcreteController> Logger { get; init; } = default!;
    [FromServices] public IOptions<IdentityOptions> IdentityOptions { get; init; } = default!;

    [NonAction]
    protected virtual string? GetUserId() => User.FindFirstValue(IdentityOptions.Value.ClaimsIdentity.UserIdClaimType);

    [NonAction]
    public virtual ActionResult InvalidTokenPayload(string? details = null) => Problem(
        title: "Invalid payload in token",
        statusCode: StatusCodes.Status400BadRequest,
        detail: details);
}