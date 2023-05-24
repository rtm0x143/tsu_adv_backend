using System.Security.Claims;
using System.Security.Cryptography;
using Auth.Features.User.Commands;
using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Common.App.Exceptions;
using Common.Domain.Exceptions;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneOf;
using EmptyResult = Common.Domain.ValueTypes.EmptyResult;

namespace Auth.Controllers
{
    public partial class UserController
    {
        /// <summary>
        /// Change banned status
        /// </summary>
        [Authorize(Roles = nameof(CommonRoles.Admin))]
        [HttpPatch("{id}/banned/{isBanned}")]
        public Task<ActionResult> BanUser(Guid id, bool isBanned, [FromServices] IChangeUserBanned banUser)
            => ExecuteRequest(banUser, new(id, isBanned));
    }
}

namespace Auth.Features.User.Commands
{
    public class ChangeUserBanned : IChangeUserBanned
    {
        private readonly AuthUserManager _userManager;

        public ChangeUserBanned(AuthUserManager userManager) => _userManager = userManager;

        public async Task<OneOf<EmptyResult, Exception>> Execute(ChangeUserBannedCommand command)
        {
            if (await _userManager.FindByIdAsync(command.UserId.ToString()) is not AppUser user)
                return new KeyNotFoundException(nameof(command.UserId));

            var banClaim = (await _userManager.GetClaimsAsync(user))
                .FirstOrDefault(claim => claim is { ValueType: CommonClaimTypes.Banned, Value: CommonBanTypes.All });

            var result = command.IsBanned switch
            {
                true when banClaim == null
                    => await _userManager.AddClaimAsync(user, new Claim(CommonClaimTypes.Banned, CommonBanTypes.All)),
                false when banClaim != null => await _userManager.RemoveClaimAsync(user, banClaim),
                _ => IdentityResult.Success
            };

            return result.Succeeded
                ? new EmptyResult()
                : UnsuitableDataException.FromIdentityResult(result);
        }
    }
}