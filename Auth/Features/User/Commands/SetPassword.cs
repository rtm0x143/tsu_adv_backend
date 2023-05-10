using Auth.Features.User.Commands;
using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Common.App.Exceptions;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Auth.Controllers
{
    public partial class UserController
    {
        [Authorize(Roles = nameof(CommonRoles.Admin))]
        [HttpPut("{userId}/password")]
        public Task<ActionResult> SetPassword(Guid userId, [FromBody] string password, [FromServices] ISetPassword setPassword)
        {
            return setPassword.Execute(new(userId, password))
                .ContinueWith(t => t.Result.IsT0
                    ? Ok()
                    : ExceptionsDescriber.Describe(t.Result.Value));
        }
    }
}

namespace Auth.Features.User.Commands
{
    public class SetPassword : ISetPassword
    {
        private readonly AuthUserManager _userManager;
        public SetPassword(AuthUserManager userManager) => _userManager = userManager;

        public async Task<OneOf<EmptyResult, Exception>> Execute(SetPasswordCommand command)
        {
            if (await _userManager.FindByIdAsync(command.UserId.ToString()) is not AppUser user)
                return new KeyNotFoundException(nameof(command.UserId));

            var result = await _userManager.SetPasswordAsync(user, command.Password);
            return !result.Succeeded
                ? UnsuitableDataException.FromIdentityResult(result)
                : new EmptyResult();
        }
    }
}