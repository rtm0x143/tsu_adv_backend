using Auth.Converters;
using Auth.Features.User.Common;
using Auth.Features.User.Queries;
using Auth.Infra.Data;
using Common.Infra.Auth;
using Common.Infra.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    public partial class UserController
    {
        [Authorize(Roles = nameof(CommonRoles.Admin))]
        [HttpGet]
        public Task<UserDataDto[]> GetUsers([FromQuery] GetUsersQuery query, [FromServices] IGetUsers getUsers)
            => getUsers.Execute(query);
    }
}

namespace Auth.Features.User.Queries
{
    public class GetUsers : IGetUsers
    {
        private readonly AuthDbContext _context;
        public GetUsers(AuthDbContext context) => _context = context;

        public Task<UserDataDto[]> Execute(GetUsersQuery query)
        {
            return _context.Users
                .Option(query.InRestaurant != default,
                    queryable => queryable.Where(
                        user => _context.RestaurantAssociationRoleClaims.Any(
                            claim => claim.RestaurantId == query.InRestaurant && claim.UserId == user.Id)))
                .Option(query.InRole != null,
                    queryable => queryable.Where(
                        user => user.Roles.Any(userRole => userRole.Role.Name == query.InRole)))
                .OrderBy(user => user.Id)
                .Option(query.Pagination.AfterRecord != default,
                    queryable => queryable.Where(user => user.Id > query.Pagination.AfterRecord))
                .Select(UserDataDtoConverter.CreateProjection(_context.UserClaims))
                .Take(query.Pagination.PageSize)
                .AsNoTracking()
                .AsSplitQuery()
                .ToArrayAsync();
        }
    }
}