using System.Security.Claims;
using Asp.Versioning;
using Auth.Features.Auth.Commands;
using Auth.Features.Auth.Common;
using Common.App.Exceptions;
using Common.App.Utils;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Auth.Controllers
{
    public partial class AuthController
    {
        [Authorize]
        [HttpDelete("logout")]
        public Task<ActionResult> Logout([FromBody] string refreshToken, [FromServices] ILogout logout)
        {
            if (!Guid.TryParse(GetUserId(), out var userId))
                return Task.FromResult(InvalidTokenPayload());

            return ExecuteRequest(logout, new(userId, refreshToken));
        }
    }
}

namespace Auth.Features.Auth.Commands
{
    public class Logout : ILogout
    {
        private readonly IRefreshTokenHandler _refreshTokenHandler;
        public Logout(IRefreshTokenHandler refreshTokenHandler) => _refreshTokenHandler = refreshTokenHandler;

        public async Task<OneOf<EmptyResult, Exception>> Execute(LogoutCommand request)
        {
            var result = await _refreshTokenHandler.Read(request.RefreshToken);
            if (!result.IsT0) return (Exception)result.Value;
            var token = result.AsT0;

            if (token.UserId != request.UserId)
                return new NotPermittedException("Given refresh token doesn't related to specified user");

            await _refreshTokenHandler.Revoke(token);
            return new EmptyResult();
        }
    }
}