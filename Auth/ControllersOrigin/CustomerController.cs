using Common.App.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Auth.ControllersOrigin;

[ApiController]
[VersioningApiRoute]
[ApiExplorerSettings(GroupName = "Auth")]
public partial class CustomerController : ControllerBase
{
    public CustomerController()
    {
    }
}