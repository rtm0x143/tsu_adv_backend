using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth.Policies;

public interface ISelfProcessingRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// Checks if <paramref name="principal"/> fits requirement
    /// </summary>
    bool IsFits(ClaimsPrincipal principal);
}