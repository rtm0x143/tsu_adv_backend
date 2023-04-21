using Auth.Features.Common;
using Auth.Features.User.Commands;
using Auth.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Auth.Controllers;

public partial class UserController
{
    [Authorize]
    [HttpPut("profile")]
    public Task<ActionResult> ChangeSelfProfile(UserProfileDto profileDto,
        [FromServices] IChangeProfile changeProfile)
    {
        if (!Guid.TryParse(GetUserId(), out var id)) return Task.FromResult(InvalidTokenPayload());

        return changeProfile.Execute(new(id, profileDto))
            .ContinueWith<ActionResult>(t => t.Result.IsT0
                ? Ok()
                : ExceptionsDescriber.Describe(t.Result.Value));
    }

    [Authorize]
    [HttpPut("{userId:guid}/profile")]
    public async Task<ActionResult> ChangeProfile(Guid userId, UserProfileDto profileDto,
        [FromServices] IChangeProfile changeProfile)
    {
        var authResult = await AuthService.AuthorizeAsync(User, null, new ChangePersonalDataRequirement(userId));
        if (!authResult.Succeeded) return Forbid();

        var result = await changeProfile.Execute(new(userId, profileDto));
        return result.IsT0 ? Ok() : ExceptionsDescriber.Describe(result.Value);
    }
}