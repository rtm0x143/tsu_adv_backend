using Common.App.Utils;
using Common.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Common.Infra.Auth.Policies;

/// <param name="DataOwnerId">Id of data owner, also could has <c>default</c> value
/// which means concrete owner cannot  be determined</param>
/// <param name="ActionType"><see cref="CommonActionTypes"/> or other extensions</param>
public record ActionOnPersonalDataRequirement(Guid DataOwnerId, string ActionType)
    : IAuthorizationRequirement
{
    public static ActionOnPersonalDataRequirement Read(Guid dataOwnerId) =>
        new(dataOwnerId, Enum.GetName(CommonActionType.Read)!);

    public static ActionOnPersonalDataRequirement Change(Guid dataOwnerId) =>
        new(dataOwnerId, Enum.GetName(CommonActionType.Change)!);
};

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