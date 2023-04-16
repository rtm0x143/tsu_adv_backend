using Common.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;

[ApiExplorerSettings(GroupName = "Auth")]
public abstract class AuthControllerBase<TConcreteController> : CommonControllerBase<TConcreteController>
    where TConcreteController : CommonControllerBase<TConcreteController>
{
    [FromServices] public IAuthorizationService AuthService { get; init; } = default!;
}