using Common.App.Attributes;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Backend.Features.Menu.Commands;

public sealed record DeleteMenuCommand(Guid RestaurantId, string Name)
    : IRequestWithException<EmptyResult, KeyNotFoundException>;

[RequestHandlerInterface]
public interface IDeleteMenu : IRequestHandlerWithException<DeleteMenuCommand, EmptyResult, KeyNotFoundException>
{
}