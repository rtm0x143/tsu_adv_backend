using Common.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;

[ApiExplorerSettings(GroupName = "Auth")]
public abstract class AuthControllerBase<TConcreteController> : CommonApiControllerBase<TConcreteController>
    where TConcreteController : CommonApiControllerBase<TConcreteController>
{
}