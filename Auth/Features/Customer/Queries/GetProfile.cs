using Auth.Features.Customer.Common;
using Auth.Infra.Data;
using Auth.Mappers.Generated;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Auth.Features.Customer.Queries
{
    public class GetProfile : IGetProfile
    {
        private readonly AuthDbContext _dbContext;
        public GetProfile(AuthDbContext dbContext) => _dbContext = dbContext;

        public Task<OneOf<CustomerProfileDto, KeyNotFoundException>> Execute(GetProfileQuery query)
        {
            return _dbContext.Customers.Where(c => c.Id == query.CustomerId)
                .Select(CustomerMapper.ProjectToProfileDto)
                .FirstOrDefaultAsync()
                .ContinueWith<OneOf<CustomerProfileDto, KeyNotFoundException>>(t => t.Result == null
                    ? new KeyNotFoundException()
                    : t.Result);
        }
    }
}