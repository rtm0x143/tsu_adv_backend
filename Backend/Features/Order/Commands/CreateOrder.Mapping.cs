using Mapster;

namespace Backend.Features.Order.Commands;

internal class DishInOrderDtoMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.DishInBasket, Domain.Services.DishInOrderDto>()
            .ShallowCopyForSameType(true)
            .GenerateMapper(MapType.Map | MapType.Projection | MapType.MapToTarget);
    }
}