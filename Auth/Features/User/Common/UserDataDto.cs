using Auth.Features.Common;
using Common.Infra.Auth;

namespace Auth.Features.User.Common;

public record UserDataDto(Guid Id, IEnumerable<string> Roles,
    KeyValuePair<string, string?>[] AllUserClaims) : UserProfileDto
{
    public Guid? Restaurant { get; } =
        Guid.TryParse(
            AllUserClaims.FirstOrDefault(claim => claim.Key == CommonClaimTypes.Restaurant).Value,
            out var restaurantId)
            ? restaurantId
            : null;

    public bool IsBanned { get; } = AllUserClaims.Any(
        claim => claim is { Key: CommonClaimTypes.Banned, Value: CommonBanTypes.All });

    // ReSharper disable once RedundantExplicitPositionalPropertyDeclaration
    public KeyValuePair<string, string?>[] AllUserClaims { get; init; } = AllUserClaims;
}