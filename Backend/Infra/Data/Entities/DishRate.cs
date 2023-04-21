namespace Backend.Infra.Data.Entities;

public class DishRate
{
    public Guid DishId { get; set; }
    public Guid UserId { get; set; }
    public float Value { get; set; }
}