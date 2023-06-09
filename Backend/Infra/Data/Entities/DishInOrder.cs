﻿namespace Backend.Infra.Data.Entities;

public class DishInOrder
{
    public Order Order { get; set; } = default!;
    public ulong OrderNumber { get; set; }
    
    public Dish Dish { get; set; } = default!;
    public Guid DishId { get; set; }
    
    public ulong Count { get; set; }
}