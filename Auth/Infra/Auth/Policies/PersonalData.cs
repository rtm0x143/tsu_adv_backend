using Common.App.Utils;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Auth.Infra.Auth.Policies;

public abstract record ActionOnPersonalDataRequirement(Guid DataOwnerId, string ActionType)
    : IAuthorizationRequirement;

public record ReadPersonalDataRequirement(Guid DataOwnerId)
    : ActionOnPersonalDataRequirement(DataOwnerId, Enum.GetName(CommonActionTypes.Read)!);

public record ChangePersonalDataRequirement(Guid DataOwnerId)
    : ActionOnPersonalDataRequirement(DataOwnerId, Enum.GetName(CommonActionTypes.Change)!);

public class PersonalDataHandler : AuthorizationHandler<ActionOnPersonalDataRequirement>
{
    private readonly IOptions<ClaimsIdentityOptions> _claimsOptions;
    public PersonalDataHandler(IOptions<ClaimsIdentityOptions> options) => _claimsOptions = options;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ActionOnPersonalDataRequirement requirement)
    {
        // Checks if caller has claim CommonClaimTypes.PersonalData to manipulate with any personal data
        if (context.User.FindFirst(claim =>
                claim.Type == CommonClaimTypes.PersonalData && claim.Value == requirement.ActionType) != null)
        {
            context.Succeed(requirement);
        }
        // Checks if user is data owner
        else if (context.User.TryFindFirstGuid(_claimsOptions.Value.UserIdClaimType, out var callerId)
                 && callerId == requirement.DataOwnerId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}