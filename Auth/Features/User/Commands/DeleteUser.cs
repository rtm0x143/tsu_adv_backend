using Auth.Features.User.Commands;
using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Common.App.Exceptions;
using Common.Domain.Exceptions;
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
        [HttpDelete("{id}")]
        public Task<ActionResult> Delete(Guid id, [FromServices] IDeleteUser deleteUser)
        {
            return deleteUser.Execute(new(id))
                .ContinueWith(t => t.Result.IsT0
                    ? Ok()
                    : ExceptionsDescriber.Describe(t.Result.Value));
        }
    }
}

namespace Auth.Features.User.Commands
{
    public class DeleteUser : IDeleteUser
    {
        private readonly AuthUserManager _userManager;
        public DeleteUser(AuthUserManager userManager) => _userManager = userManager;

        public async Task<OneOf<EmptyResult, Exception>> Execute(DeleteUserCommand command)
        {
            if (await _userManager.FindByIdAsync(command.Id.ToString()) is not AppUser user)
                return new KeyNotFoundException(nameof(command.Id));

            var result = await _userManager.DeleteAsync(user);
            // In fact in only can be Concurrency error
            if (!result.Succeeded) return new ActionFailedException(result.Errors.First().Description);
            return new EmptyResult();
        }
    }
}