using Auth.Features.Common;
using Auth.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Auth.Features.User.Queries;
using Common.Infra.Auth.Policies;

// ReSharper disable once CheckNamespace
namespace Auth.Controllers;

public partial class UserController
{
    [Authorize]
    [HttpGet("profile")]
    public Task<ActionResult<UserProfileDto>> GetSelfProfile([FromServices] IGetProfile getProfile)
    {
        if (!Guid.TryParse(GetUserId(), out var id))
            return Task.FromResult<ActionResult<UserProfileDto>>(
                BadRequest("Invalid user identifier in token"));

        return getProfile.Execute(new(id))
            .ContinueWith<ActionResult<UserProfileDto>>(t => t.Result.IsT0
                ? Ok(t.Result.AsT0)
                : NotFound());
    }

    [Authorize]
    [HttpGet("{userId:guid}/profile")]
    public async Task<ActionResult<UserProfileDto>> GetProfile(Guid userId, [FromServices] IGetProfile getProfile)
    {
        var authResult = await AuthService.AuthorizeAsync(User, null, ActionOnPersonalDataRequirement.Read(userId));
        if (!authResult.Succeeded) return Forbid();

        var result = await getProfile.Execute(new(userId));
        return result.IsT0 ? Ok(result.AsT0) : NotFound();
    }
}