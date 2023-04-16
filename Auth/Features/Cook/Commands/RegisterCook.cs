using Auth.Features.Cook.Commands;
using Auth.Features.User.Commands;
using Auth.Infra.Auth.Policies;
using Auth.Mappers.Generated;
using Common.App.Models.Results;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.Controllers
{
    public partial class CookController
    {
        /// <summary>
        /// Registers new user as a Cook
        /// </summary>
        /// <responce code="400">When some data were unsuitable</responce>
        /// <responce code="403">When caller has not enough permissions</responce>
        /// <returns>New user's id</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IdResult>> Register(CookRegistrationDto dto,
            [FromServices] IRegisterCook useCase)
        {
            var authResult = await AuthService.AuthorizeAsync(User, null,
                new GrantInRestaurantRequirement(dto.RestaurantId, CommonRoles.RestaurantOwner));
            if (!authResult.Succeeded) return Forbid();

            var result = await useCase.Execute(new RegisterCookCommand(dto));
            return result.IsT0 ? Ok(result.AsT0) : ExceptionsDescriber.Describe(result.Value);
        }
    }
}

namespace Auth.Features.Cook.Commands
{
    public class RegisterCook : IRegisterCook
    {
        private readonly IRegisterRestaurantRelatedUser _register;
        public RegisterCook(IRegisterRestaurantRelatedUser register) => _register = register;

        public Task<OneOf<IdResult, Exception>> Execute(RegisterCookCommand command)
        {
            var newUser = new Infra.Data.Entities.Cook();
            command.CookDto.AdaptTo(newUser);
            return _register.Execute(
                new(newUser, command.CookDto.Password, CommonRoles.Cook, command.CookDto.RestaurantId));
        }
    }
}