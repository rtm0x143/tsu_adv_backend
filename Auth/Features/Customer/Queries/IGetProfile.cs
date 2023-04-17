using Auth.Features.Customer.Common;
using Common.App.Attributes;
using Common.App.RequestHandlers;

namespace Auth.Features.Customer.Queries;

public sealed record GetProfileQuery(Guid CustomerId) : IRequestWithException<CustomerProfileDto, KeyNotFoundException>;

[RequestHandlerInterface]
public interface IGetProfile : IRequestHandlerWithException<GetProfileQuery, CustomerProfileDto, KeyNotFoundException>
{
}