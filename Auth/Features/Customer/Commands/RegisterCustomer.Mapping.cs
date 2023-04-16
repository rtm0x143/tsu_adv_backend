using Auth.Features.Common;
using Auth.Infra.Data.Entities;
using Mapster;

namespace Auth.Features.Customer.Commands;

// ReSharper disable once UnusedType.Global
internal class CustomerRegistrationDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CustomerRegistrationDto, Infra.Data.Entities.Customer>()
            .GenerateMapper(MapType.Map | MapType.MapToTarget);
    }
}