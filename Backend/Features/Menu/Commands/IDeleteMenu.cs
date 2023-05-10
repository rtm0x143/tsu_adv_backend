using Common.App.Attributes;
using Common.App.RequestHandlers;
using Common.Domain.ValueTypes;

namespace Backend.Features.Menu.Commands;

public sealed record DeleteMenuCommand(Guid RestaurantId, string Name)
    : IRequestWithException<EmptyResult, KeyNotFoundException>;

[RequestHandlerInterface]
public interface IDeleteMenu : IRequestHandlerWithException<DeleteMenuCommand, EmptyResult, KeyNotFoundException>
{
}