using Auth.Features.Customer.Common;
using Auth.Features.Customer.Queries;
using Auth.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Auth.Controllers;

public partial class CustomerController
{
    [Authorize]
    [HttpGet("profile")]
    public Task<ActionResult<CustomerProfileDto>> GetSelfProfile([FromServices] IGetProfile getProfile)
    {
        if (!Guid.TryParse(GetUserId(), out var id))
            return Task.FromResult<ActionResult<CustomerProfileDto>>(
                BadRequest("Invalid user identifier in token"));

        return getProfile.Execute(new(id))
            .ContinueWith<ActionResult<CustomerProfileDto>>(t => t.Result.IsT0
                ? Ok(t.Result.AsT0)
                : NotFound());
    }

    [Authorize]
    [HttpGet("{customerId:guid}/profile")]
    public async Task<ActionResult<CustomerProfileDto>> GetProfile(Guid customerId,
        [FromServices] IGetProfile getProfile)
    {
        var authResult = await AuthService.AuthorizeAsync(User, null, new ReadPersonalDataRequirement(customerId));
        if (!authResult.Succeeded) return Forbid();

        var result = await getProfile.Execute(new(customerId));
        return result.IsT0 ? Ok(result.AsT0) : NotFound();
    }
}