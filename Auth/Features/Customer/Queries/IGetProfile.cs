using Auth.Features.Common;
using Auth.Infra.Data.Entities;
using Common.App.Attributes;
using Common.App.RequestHandlers;
using Mapster;

namespace Auth.Features.Customer.Queries;

public record CustomerProfileDto(
        string Fullname,
        string Email,
        string PhoneNumber,
        Gender Gender,
        DateOnly? BirthDate,
        string Address)
    : UserProfileDto(Fullname, Email, PhoneNumber, Gender, BirthDate);

public sealed record GetProfileQuery(Guid CustomerId) : IRequestWithException<CustomerProfileDto, KeyNotFoundException>;

[RequestHandlerInterface]
public interface IGetProfile : IRequestHandlerWithException<GetProfileQuery, CustomerProfileDto, KeyNotFoundException>
{
}