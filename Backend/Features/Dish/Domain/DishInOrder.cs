namespace Backend.Features.Dish.Domain;

public class DishInOrder
{
    public Guid DishId { get; }
    public ulong OrderNumber { get; }
    public Order Order { get; } = null!;

    public DishInOrder(Guid dishId, Order order)
    {
        DishId = dishId;
        Order = order;
        OrderNumber = order.Number;
    }

    private DishInOrder() // for EF 
    {
    }
}