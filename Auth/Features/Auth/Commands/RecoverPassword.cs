using System.Security.Authentication;
using Auth.Features.Auth.Commands;
using Auth.Features.Auth.Common;
using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Common.App.Exceptions;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using EmptyResult = Common.App.Models.Results.EmptyResult;

namespace Auth.Controllers
{
    public partial class AuthController
    {
        /// <summary>
        /// Use password recovery token
        /// </summary>
        /// <responce code="200">Password recovered</responce>
        /// <responce code="403">Recovery token was invalid</responce>
        /// <responce code="400">New password token was invalid</responce>
        /// <remarks>If succeeded, all refresh tokens will be invalidated</remarks>
        [HttpPut("password/recover")] 
        public Task<ActionResult> RecoverPassword(RecoverPasswordCommand command,
            [FromServices] IRecoverPassword recoverPassword)
        {
            return recoverPassword.Execute(command)
                .ContinueWith(t => t.Result.IsT0
                    ? Ok()
                    : ExceptionsDescriber.Describe(t.Result.Value));
        }
    }
}

namespace Auth.Features.Auth.Commands
{
    public class RecoverPassword : IRecoverPassword
    {
        private readonly AuthUserManager _userManager;
        private readonly IRefreshTokenHandler _refreshTokenHandler;

        public RecoverPassword(AuthUserManager userManager, IRefreshTokenHandler refreshTokenHandler)
        {
            _userManager = userManager;
            _refreshTokenHandler = refreshTokenHandler;
        }

        public async Task<OneOf<EmptyResult, Exception>> Execute(RecoverPasswordCommand command)
        {
            if (await _userManager.FindByEmailAsync(command.UsersEmail) is not AppUser user)
                return new KeyNotFoundException(nameof(command.UsersEmail));

            var resetResult = await _userManager
                .ResetPasswordAsync(user, command.ChangePasswordToken, command.NewPassword);

            if (!resetResult.Succeeded)
            {
                var tokenInvalid = resetResult.Errors.FirstOrDefault(e =>
                    e.Code == nameof(_userManager.ErrorDescriber.InvalidToken)) != null;
                return tokenInvalid
                    ? new InvalidCredentialException(nameof(command.ChangePasswordToken))
                    : UnsuitableDataException.FromIdentityResult(resetResult);
            }
            
            await _refreshTokenHandler.DropFamily(user);
            return new EmptyResult();
        }
    }
}