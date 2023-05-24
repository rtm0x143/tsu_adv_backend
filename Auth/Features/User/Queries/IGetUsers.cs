using Auth.Features.User.Common;
using Common.App.Attributes;
using Common.App.Dtos;
using Common.App.RequestHandlers;

namespace Auth.Features.User.Queries;

public sealed record GetUsersQuery(PaginationInfo<Guid> Pagination, string? InRole = null, Guid InRestaurant = default)
    : IRequest<UserDataDto[]>;

[RequestHandlerInterface]
public interface IGetUsers : IRequestHandler<GetUsersQuery, UserDataDto[]>
{
}