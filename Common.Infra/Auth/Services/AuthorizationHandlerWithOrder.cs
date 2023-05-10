using Common.Domain.Utils;
using Microsoft.AspNetCore.Authorization;

namespace Common.Infra.Auth.Services;

/// <summary>
/// That type of handlers will be ordered by <see cref="OrderingAuthorizationHandlerProvider"/> 
/// </summary>
public abstract class AuthorizationHandlerWithOrder<TRequirement> : AuthorizationHandler<TRequirement>, IHasOrderFactor
    where TRequirement : IAuthorizationRequirement
{
    public abstract int OrderFactor { get; }
}