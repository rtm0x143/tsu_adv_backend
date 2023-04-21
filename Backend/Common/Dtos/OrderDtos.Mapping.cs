using Backend.Infra.Data.Entities;
using Mapster;

namespace Backend.Common.Dtos;

public class OrderShortDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Order, OrderShortDto>()
            .GenerateMapper(MapType.Projection | MapType.Map | MapType.MapToTarget);
    }
}