using Auth.Infra.Data.Entities;
using Mapster;

namespace Auth.Features.User.Commands;

internal class RegisterUserDtoMapper : IRegister
{
    public virtual void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterUserDto, AppUser>()
            .Map(d => d.UserName, s => s.Fullname)
            .GenerateMapper(MapType.Map | MapType.MapToTarget);
    }
}