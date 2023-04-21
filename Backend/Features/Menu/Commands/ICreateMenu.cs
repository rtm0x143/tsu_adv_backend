using Common.App.Attributes;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Backend.Features.Menu.Commands;

public record MenuCreationDto(string Name);

/// <exception cref="KeyNotFoundException"><paramref name="RestaurantId"/> not found</exception>
/// <exception cref="CollisionException"></exception>
public sealed record CreateMenuCommand(Guid RestaurantId, MenuCreationDto Menu) : IRequestWithException<EmptyResult>;

[RequestHandlerInterface]
public interface ICreateMenu : IRequestHandlerWithException<CreateMenuCommand, EmptyResult>
{
}