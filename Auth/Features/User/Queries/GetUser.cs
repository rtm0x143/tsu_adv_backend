using Auth.Converters;
using Auth.Features.User.Common;
using Auth.Features.User.Queries;
using Auth.Infra.Data;
using Auth.Infra.Data.Entities;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace Auth.Controllers
{
    public partial class UserController
    {
        [Authorize(Roles = nameof(CommonRoles.Admin))]
        [HttpGet("{id}")]
        public Task<ActionResult<UserDataDto>> GetUser(Guid id, [FromServices] IGetUser getUser)
            => ExecuteRequest(getUser, new(id));
    }
}

namespace Auth.Features.User.Queries
{
    public class GetUser : IGetUser
    {
        private readonly AuthDbContext _context;

        public GetUser(AuthDbContext context) => _context = context;

        public Task<OneOf<UserDataDto, KeyNotFoundException>> Execute(GetUserQuery query)
        {
            return _context.Users.Where(user => user.Id == query.Id)
                .Select(UserDataDtoConverter.CreateProjection(_context.UserClaims))
                .AsNoTracking()
                .FirstOrDefaultAsync()
                .ContinueWith<OneOf<UserDataDto, KeyNotFoundException>>(task => task.Result != null
                    ? task.Result
                    : new KeyNotFoundException(nameof(query.Id)));
        }
    }
}