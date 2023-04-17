using Auth.Features.Customer.Commands;
using Auth.Features.Customer.Common;
using Auth.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Auth.Controllers;

public partial class CustomerController
{
    [Authorize]
    [HttpPut("profile")]
    public Task<ActionResult> ChangeSelfProfile(CustomerProfileDto profileDto,
        [FromServices] IChangeProfile changeProfile)
    {
        if (!Guid.TryParse(GetUserId(), out var id))
            return Task.FromResult<ActionResult>(BadRequest("Invalid user identifier in token"));

        return changeProfile.Execute(new(id, profileDto))
            .ContinueWith<ActionResult>(t => t.Result.IsT0
                ? Ok()
                : ExceptionsDescriber.Describe(t.Result.Value));
    }

    [Authorize]
    [HttpPut("{customerId:guid}/profile")]
    public async Task<ActionResult> ChangeProfile(Guid customerId,
        CustomerProfileDto profileDto,
        [FromServices] IChangeProfile changeProfile)
    {
        var authResult = await AuthService.AuthorizeAsync(User, null, new ChangePersonalDataRequirement(customerId));
        if (!authResult.Succeeded) return Forbid();

        var result = await changeProfile.Execute(new(customerId, profileDto));
        return result.IsT0 ? Ok() : ExceptionsDescriber.Describe(result.Value);
    }
}