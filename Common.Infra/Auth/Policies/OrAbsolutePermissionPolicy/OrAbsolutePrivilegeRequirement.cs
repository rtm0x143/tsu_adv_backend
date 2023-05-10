using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth.Policies;

/// <summary>
/// If user satisfies all <paramref name="AbsolutePrivilegeRequirements"/> then all other requirement will be marked as succeeded  
/// </summary>
/// <remarks>
/// If requirement fulfil but context <see cref="AuthorizationHandlerContext.HasFailed"/> then authorization fails
/// </remarks>
public sealed record OrAbsolutePrivilegeRequirement(
        IEnumerable<IAuthorizationRequirement> AbsolutePrivilegeRequirements)
    : IAuthorizationRequirement;