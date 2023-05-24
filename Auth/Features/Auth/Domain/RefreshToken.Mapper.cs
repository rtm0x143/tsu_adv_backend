using Auth.Infra.Data.Entities;
using Mapster;

namespace Auth.Features.Auth.Common;

public class RefreshTokenMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RefreshToken, UserRefreshToken>()
            .GenerateMapper(MapType.Map | MapType.MapToTarget);
    }
}