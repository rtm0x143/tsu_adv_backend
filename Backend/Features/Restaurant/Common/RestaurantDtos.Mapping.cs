using Mapster;

namespace Backend.Features.Restaurant.Common;

internal class RestaurantDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Infra.Data.Entities.Restaurant, RestaurantDto>()
            .TwoWays()
            .GenerateMapper(MapType.Map | MapType.MapToTarget | MapType.Projection);
    }
}