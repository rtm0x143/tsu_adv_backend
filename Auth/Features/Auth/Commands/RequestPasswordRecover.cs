using Auth.Features.Auth.Commands;
using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Common.App.Exceptions;
using Common.App.Services;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Auth.Controllers
{
    public partial class AuthController
    {
        [HttpPost("password/recover")]
        public Task<ActionResult> RequestPasswordRecover(RequestPasswordRecoverCommand command,
            [FromServices] IRequestPasswordRecover requestPasswordRecover)
        {
            return requestPasswordRecover.Execute(command)
                .ContinueWith(t => t.Result.IsT0
                    ? Ok()
                    : ExceptionsDescriber.Describe(t.Result.Value));
        }
    }
}

namespace Auth.Features.Auth.Commands
{
    public class RequestPasswordRecover : IRequestPasswordRecover
    {
        private readonly AuthUserManager _userManager;
        private readonly PasswordRecoveryMailSender _passwordRecoveryMailSender;

        public RequestPasswordRecover(AuthUserManager userManager, IMailSender mailSender)
        {
            _userManager = userManager;
            _passwordRecoveryMailSender = new PasswordRecoveryMailSender { Sender = mailSender };
        }

        public async Task<OneOf<EmptyResult, KeyNotFoundException>> Execute(RequestPasswordRecoverCommand request)
        {
            if (await _userManager.FindByEmailAsync(request.UsersEmail) is not AppUser user)
                return new KeyNotFoundException(nameof(request.UsersEmail));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var sendResult = await _passwordRecoveryMailSender.SendRecoveryMessageFor(user, token);
            if (!sendResult.IsT0)
                throw new UnexpectedException("While sending password recovery code", sendResult.AsT1);
            return new EmptyResult();
        }
    }
}