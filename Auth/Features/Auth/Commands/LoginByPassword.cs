using Auth.Features.Auth.Commands;
using Auth.Features.Auth.Common;
using Auth.Features.Common;
using Auth.Infra.Data.Entities;
using Common.App.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.Controllers
{
    public partial class AuthController
    {
        [HttpPost("login")]
        public Task<ActionResult<TokensResult>> Login(LoginByPasswordCommand command,
            [FromServices] ILoginByPassword login)
        {
            return login.Execute(command).ContinueWith<ActionResult<TokensResult>>(t => t.Result.IsT0
                ? t.Result.AsT0
                : Unauthorized());
        }
    }
}

namespace Auth.Features.Auth.Commands
{
    // ReSharper disable once UnusedType.Global
    public class LoginByPassword : ILoginByPassword
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IRefreshTokenHandler _refreshTokenHandler;

        public LoginByPassword(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator,
            SignInManager<AppUser> signInManager, IRefreshTokenHandler refreshTokenHandler)
        {
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _signInManager = signInManager;
            _refreshTokenHandler = refreshTokenHandler;
        }

        public async Task<OneOf<TokensResult, KeyNotFoundException, ActionFailedException>> Execute(
            LoginByPasswordCommand command)
        {
            if (await _userManager.FindByEmailAsync(command.Email) is not AppUser user)
                return new KeyNotFoundException(nameof(command.Email));

            if (!await _userManager.CheckPasswordAsync(user, command.Password))
                return new ActionFailedException();

            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            var access = _jwtGenerator.IssueToken(principal.Claims);

            var refreshTokenResult = await _refreshTokenHandler.IssuerFor(user);
            if (refreshTokenResult.IsT1) return new ActionFailedException(refreshTokenResult.AsT1.Message);
            var refresh = _refreshTokenHandler.Write(refreshTokenResult.AsT0);

            return new TokensResult(access, refresh);
        }
    }
}