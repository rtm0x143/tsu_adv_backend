using Auth.Features.RestaurantOwner.Commands;
using Auth.Features.User.Commands;
using Auth.Infra.Auth.Policies;
using Auth.Converters;
using Common.Domain.ValueTypes;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.Controllers
{
    public partial class RestaurantOwnerController
    {
        /// <summary>
        /// Registers new user as a RestaurantOwner
        /// </summary>
        /// <responce code="400">When some data were unsuitable</responce>
        /// <responce code="403">When caller has not enough permissions</responce>
        /// <returns>New user's id</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IdResult>> Register(RestaurantOwnerRegistrationDto dto,
            [FromServices] IRegisterOwner useCase)
        {
            var authResult = await AuthService.AuthorizeAsync(User, null,
                new GrantInRestaurantRequirement(dto.RestaurantId, CommonRoles.RestaurantOwner));
            if (!authResult.Succeeded) return Forbid();

            var result = await useCase.Execute(new RegisterOwnerCommand(dto));
            return result.IsT0 ? Ok(result.AsT0) : ExceptionsDescriber.Describe(result.Value);
        }
    }
}

namespace Auth.Features.RestaurantOwner.Commands
{
    public class RegisterOwner : IRegisterOwner
    {
        private readonly IRegisterRestaurantRelatedUser _register;
        public RegisterOwner(IRegisterRestaurantRelatedUser register) => _register = register;

        public Task<OneOf<IdResult, Exception>> Execute(RegisterOwnerCommand command)
        {
            var newUser = new Infra.Data.Entities.RestaurantAdmin();
            command.OwnerDto.AdaptTo(newUser);

            return _register.Execute(
                new(newUser, command.OwnerDto.Password, CommonRoles.RestaurantOwner, command.OwnerDto.RestaurantId));
        }
    }
}