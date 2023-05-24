using Auth.Features.User.Common;
using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Auth.Features.User.Queries;

public sealed record GetUserQuery(Guid Id) : IRequestWithException<UserDataDto, KeyNotFoundException>;

[RequestHandlerInterface]
public interface IGetUser : IRequestHandlerWithException<GetUserQuery, UserDataDto, KeyNotFoundException>
{
}