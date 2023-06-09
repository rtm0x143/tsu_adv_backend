﻿using Backend.Common.Dtos;
using Common.App.Attributes;
using Common.App.Dtos;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Order.Commands;

/// <param name="OrderNumber">Number of order to duplicate</param>
/// <param name="DeliveryTime">New delivery time</param>
/// <param name="UserId">Id of the user who creates new order</param>
/// <param name="Address">In not specified use calling customer address</param>
/// <exception cref="KeyNotFoundException">if <paramref name="OrderNumber"/> not found</exception>
public sealed record RepeatOrderCommand(ulong OrderNumber, DateTime DeliveryTime, string? Address = null,
    Guid UserId = default) : IRequestWithException<IdResult<ulong>>;

[RequestHandlerInterface]
public interface IRepeatOrder : IRequestHandlerWithException<RepeatOrderCommand, IdResult<ulong>>
{
}