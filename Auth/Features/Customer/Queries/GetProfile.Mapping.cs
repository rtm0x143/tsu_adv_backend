using Auth.Features.Common;
using Auth.Infra.Data.Entities;
using Mapster;

namespace Auth.Features.Customer.Queries;

internal class CustomerProfileDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CustomerProfileDto, Infra.Data.Entities.Customer>()
            .Inherits<UserProfileDto, AppUser>()
            .TwoWays()
            .GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
    }
}