using Auth.Features.Auth.Common;
using Auth.Infra.Data.Entities;

namespace Auth.Mappers.Generated
{
    public static partial class RefreshTokenMapper
    {
        public static UserRefreshToken AdaptToUser(this RefreshToken p1)
        {
            return p1 == null ? null : new UserRefreshToken()
            {
                Id = p1.Id,
                UserId = p1.UserId,
                ExpiresAt = p1.ExpiresAt,
                IsUsed = p1.IsUsed
            };
        }
        public static UserRefreshToken AdaptTo(this RefreshToken p2, UserRefreshToken p3)
        {
            if (p2 == null)
            {
                return null;
            }
            UserRefreshToken result = p3 ?? new UserRefreshToken();
            
            result.Id = p2.Id;
            result.UserId = p2.UserId;
            result.ExpiresAt = p2.ExpiresAt;
            result.IsUsed = p2.IsUsed;
            return result;
            
        }
    }
}