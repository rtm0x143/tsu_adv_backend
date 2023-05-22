using Auth.Features.Auth.Commands;
using Auth.Features.Auth.Common;
using Auth.Features.Common;
using Auth.Infra.Data.Entities;
using Common.App.Exceptions;
using Common.App.Utils;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.Controllers
{
    public partial class AuthController
    {
        [HttpPost("refresh")]
        public Task<ActionResult<TokensResult>> Refresh(RefreshCommand command, [FromServices] IRefresh refresh)
        {
            return refresh.Execute(command).ContinueWith(task => task.Result.Match<ActionResult<TokensResult>>(
                tokens => Ok(tokens),
                exception => exception switch
                {
                    ActionFailedException => Unauthorized(exception.Message),
                    _ => ExceptionsDescriber.Describe(exception)
                }));
        }
    }
}

namespace Auth.Features.Auth.Commands
{
    // ReSharper disable once UnusedType.Global
    public class Refresh : IRefresh
    {
        private readonly IRefreshTokenHandler _refreshTokenHandler;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly SignInManager<AppUser> _signInManager;

        public Refresh(IRefreshTokenHandler refreshTokenHandler, IJwtGenerator jwtGenerator,
            SignInManager<AppUser> signInManager)
        {
            _refreshTokenHandler = refreshTokenHandler;
            _jwtGenerator = jwtGenerator;
            _signInManager = signInManager;
        }

        public async Task<OneOf<TokensResult, Exception>> Execute(RefreshCommand request)
        {
            var result = await _refreshTokenHandler.Read(request.RefreshToken);
            if (!result.TryGetValue(out var tokenModel, out var tokenException)) return tokenException;

            var refreshResult = await tokenModel.ExecuteRefresh(_refreshTokenHandler);
            if (!refreshResult.TryGetValue(out var newRefreshToken, out var refreshException)) return refreshException;

            var user = await _signInManager.UserManager.FindByIdAsync(newRefreshToken.UserId.ToString());
            if (user == null) return new KeyNotFoundException(nameof(newRefreshToken.UserId));

            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            return new TokensResult(_jwtGenerator.IssueToken(principal.Claims),
                _refreshTokenHandler.Write(newRefreshToken));
        }
    }
}