using Mapster;
using Entities = Backend.Infra.Data.Entities;

namespace Backend.Features.Dish.Queries;

public class DishDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Entities.Dish, DishDto>()
            .Map(dto => dto.Rate, entity => entity.CachedRate)
            .GenerateMapper(MapType.Map | MapType.Projection);
    }
}