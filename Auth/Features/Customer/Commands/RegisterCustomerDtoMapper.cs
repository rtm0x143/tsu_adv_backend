using Auth.Infra.Data.Entities;
using Auth.Mappers.Generated;
using Mapster;

namespace Auth.Features.Customer.Commands;

internal class RegisterCustomerDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterCustomerDto, CustomerData>()
            .BeforeMapping((src, dest) => src.AdaptTo(dest.User))
            .GenerateMapper(MapType.Map | MapType.MapToTarget);
    }
}