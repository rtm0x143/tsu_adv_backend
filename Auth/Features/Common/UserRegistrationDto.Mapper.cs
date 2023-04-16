using Auth.Features.Customer.Commands;
using Auth.Infra.Data.Entities;
using Mapster;

namespace Auth.Features.Common;

// ReSharper disable once UnusedType.Global
internal class UserRegistrationDtoMapper : IRegister
{
    public virtual void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserRegistrationDto, AppUser>()
            .Map(d => d.UserName, s => s.Fullname)
            .Include<CustomerRegistrationDto, Infra.Data.Entities.Customer>()
            .GenerateMapper(MapType.Map | MapType.MapToTarget);
    }
}