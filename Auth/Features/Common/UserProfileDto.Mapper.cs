using Auth.Features.Customer.Queries;
using Auth.Infra.Data.Entities;
using Mapster;

namespace Auth.Features.Common;

internal class UserProfileDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserProfileDto, AppUser>()
            .TwoWays()
            .Map(dest => dest.UserName, src => src.Fullname)
            .GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
    }
}