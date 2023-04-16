using Auth.Features.Common;
using Auth.Infra.Data.Entities;
using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Auth.Features.User.Queries;

public sealed record GetProfileQuery(Guid CustomerId) : IRequestWithException<UserProfileDto, KeyNotFoundException>;

[RequestHandlerInterface]
public interface IGetProfile : IRequestHandlerWithException<GetProfileQuery, UserProfileDto, KeyNotFoundException>
{
}