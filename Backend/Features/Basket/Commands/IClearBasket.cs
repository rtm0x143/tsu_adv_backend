using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Backend.Features.Basket.Commands;

public sealed record ClearBasketCommand(Guid UserId) : IRequest;

[RequestHandlerInterface]
public interface IClearBasket : IRequestHandler<ClearBasketCommand>
{
}