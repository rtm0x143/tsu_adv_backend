
using Auth.Features.Common;
using Auth.Infra.Data;
using Auth.Converters;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Auth.Features.User.Queries
{
    public class GetProfile : IGetProfile
    {
        private readonly AuthDbContext _dbContext;
        public GetProfile(AuthDbContext dbContext) => _dbContext = dbContext;

        public Task<OneOf<UserProfileDto, KeyNotFoundException>> Execute(GetProfileQuery query)
        {
            return _dbContext.Users.Where(c => c.Id == query.CustomerId)
                .Select(AppUserMapper.ProjectToUserProfileDto)
                .FirstOrDefaultAsync()
                .ContinueWith<OneOf<UserProfileDto, KeyNotFoundException>>(t => t.Result == null
                    ? new KeyNotFoundException()
                    : t.Result);
        }
    }
}