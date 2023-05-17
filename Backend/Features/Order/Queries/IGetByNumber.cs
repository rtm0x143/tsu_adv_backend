using Backend.Common.Dtos;
using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Backend.Features.Order.Queries;

public sealed record GetByNumberQuery(ulong Number) : IRequestWithException<OrderDto>;

[RequestHandlerInterface]
public interface IGetByNumber : IRequestHandlerWithException<GetByNumberQuery, OrderDto>
{
}