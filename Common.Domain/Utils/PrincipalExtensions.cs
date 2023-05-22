using System.Security.Claims;
using Common.Domain.Exceptions;

namespace Common.Domain.Utils;

public static class PrincipalExtensions
{
    /// <summary>
    /// Try find first <paramref name="claimType"/>'s value what is valid <see cref="Guid"/>
    /// </summary>
    /// <returns><c>true</c> when <paramref name="claimType"/> found <c>false</c> otherwise</returns>
    public static bool TryFindFirstGuid(this ClaimsPrincipal principal, string claimType, out Guid guid) =>
        Guid.TryParse(principal.FindFirst(claimType)?.Value, out guid);

    /// <summary>
    /// Find first <paramref name="claimType"/>'s value and parse it as <see cref="Guid"/>
    /// </summary>
    /// <remarks>
    /// See exceptions in <see cref="Guid.Parse(System.ReadOnlySpan{char})"/>
    /// </remarks>
    /// <returns><see cref="Guid"/> when <paramref name="claimType"/> found <c>null</c> otherwise</returns>
    public static Guid? FindFirstAsGuid(this ClaimsPrincipal principal, string claimType)
        => principal.FindFirst(claimType)?.Value is string value ? Guid.Parse(value) : null;

    public static Guid GetRequiredUserId(this ClaimsPrincipal principal)
        => principal.FindFirstAsGuid(ClaimTypes.NameIdentifier)
           ?? throw new InvalidUserPrincipalException();
}