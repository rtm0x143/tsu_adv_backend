using Auth.Features.Auth.Commands;
using Auth.Features.Auth.Common;
using Auth.Infra.Data;
using Auth.Infra.Data.Entities;
using Common.App.Exceptions;
using Common.Infra.Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.Controllers
{
    public partial class UserController
    {
        [Authorize]
        [HttpPut("password")]
        public Task<ActionResult<TokensResult>> ChangePassword(ChangePasswordDto dto,
            [FromServices] IChangePassword changePassword)
        {
            if (!Guid.TryParse(GetUserId(), out var id))
                return Task.FromResult<ActionResult<TokensResult>>(BadRequest("Invalid token payload"));

            return changePassword.Execute(new(id, dto.CurrentPassword, dto.NewPassword))
                .ContinueWith<ActionResult<TokensResult>>(t => t.Result.IsT0
                    ? Ok()
                    : ExceptionsDescriber.Describe(t.Result.Value));
        }
    }
}

namespace Auth.Features.Auth.Commands
{
    public class ChangePassword : IChangePassword
    {
        private readonly IRefreshTokenHandler _refreshTokenHandler;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AuthDbContext _dbContext;

        public ChangePassword(IRefreshTokenHandler refreshTokenHandler,
            IJwtGenerator jwtGenerator, SignInManager<AppUser> signInManager, AuthDbContext dbContext)
        {
            _refreshTokenHandler = refreshTokenHandler;
            _jwtGenerator = jwtGenerator;
            _signInManager = signInManager;
            _dbContext = dbContext;
        }

        public async Task<OneOf<TokensResult, Exception>> Execute(ChangePasswordCommand command)
        {
            if (await _signInManager.UserManager.FindByIdAsync(command.UserId.ToString()) is not AppUser user)
                return new KeyNotFoundException(nameof(command.UserId));

            await using var tr = await _dbContext.Database.BeginNestingTransaction();

            var result = await _signInManager.UserManager
                .ChangePasswordAsync(user, command.CurrentPassword, command.NewPassword);
            if (!result.Succeeded) return new ActionFailedException();

            await _refreshTokenHandler.DropFamily(user);

            var refreshIssueResult = await _refreshTokenHandler.IssuerFor(user);
            if (!refreshIssueResult.IsT0)
            {
                await tr.RollbackAsync();
                return (Exception)refreshIssueResult.Value;
            }

            var claims = await _signInManager.ClaimsFactory.CreateAsync(user);
            var tokens = new TokensResult(_jwtGenerator.IssueToken(claims.Claims),
                _refreshTokenHandler.Write(refreshIssueResult.AsT0));

            await tr.CommitAsync();
            return tokens;
        }
    }
}