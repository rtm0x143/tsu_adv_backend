using Backend.Controllers;
using Backend.Infra.Data.Enums;

namespace Backend.Common.Dtos;

public record OrderShortDto(OrderNumber Number, decimal Price, string Address, OrderStatus Status);