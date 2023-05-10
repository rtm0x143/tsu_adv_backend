using Auth.Features.Auth.Commands;
using Auth.Features.Auth.Common;
using Auth.Features.Common;
using Auth.Infra.Data.Entities;
using Common.App.Exceptions;
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
            return refresh.Execute(command).ContinueWith(t => t.Result.Match<ActionResult<TokensResult>>(
                tokens => Ok(tokens),
                actionFailed => Unauthorized(actionFailed.Message),
                argument => BadRequest(argument.Message),
                notFound => NotFound(notFound.Message)));
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

        public async Task<OneOf<TokensResult, ActionFailedException, ArgumentException, KeyNotFoundException>> Execute(
            RefreshCommand request)
        {
            var result = await _refreshTokenHandler.Read(request.RefreshToken);
            if (!result.TryPickT0(out var tokenModel, out var rem)) return rem.IsT1 ? rem.AsT1 : rem.AsT0;

            var refreshResult = await tokenModel.ExecuteRefresh(_refreshTokenHandler);
            if (!refreshResult.TryPickT0(out var newRefreshToken, out var refreshException)) return refreshException;

            var user = await _signInManager.UserManager.FindByIdAsync(newRefreshToken.UserId.ToString());
            if (user == null) return new KeyNotFoundException("UserId");

            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            return new TokensResult(_jwtGenerator.IssueToken(principal.Claims),
                _refreshTokenHandler.Write(newRefreshToken));
        }
    }
}