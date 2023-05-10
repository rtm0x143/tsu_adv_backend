using Auth.Features.Manager.Commands;
using Auth.Features.User.Commands;
using Auth.Infra.Auth.Policies;
using Auth.Converters;
using Common.App.Utils;
using Common.Domain.ValueTypes;
using Common.Infra.Auth;
using Common.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.Controllers
{
    public partial class ManagerController
    {
        /// <summary>
        /// Registers new user as a Manager
        /// </summary>
        /// <responce code="400">When some data were unsuitable</responce>
        /// <responce code="403">When caller has not enough permissions</responce>
        /// <returns>New user's id</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IdResult>> Register(ManagerRegistrationDto dto,
            [FromServices] IRegisterManager useCase)
        {
            var authResult = await AuthService.AuthorizeAsync(User, null,
                new GrantInRestaurantRequirement(dto.RestaurantId, CommonRoles.RestaurantOwner));
            if (!authResult.Succeeded) return Forbid();

            var result = await useCase.Execute(new RegisterManagerCommand(dto));
            return result.IsT0 ? Ok(result.AsT0) : ExceptionsDescriber.Describe(result.Value);
        }
    }
}

namespace Auth.Features.Customer.Commands
{
    public class RegisterManager : IRegisterManager
    {
        private readonly IRegisterRestaurantRelatedUser _register;
        public RegisterManager(IRegisterRestaurantRelatedUser register) => _register = register;

        public Task<OneOf<IdResult, Exception>> Execute(RegisterManagerCommand command)
        {
            var newUser = new Infra.Data.Entities.Manager();
            command.ManagerDto.AdaptTo(newUser);

            return _register.Execute(
                new(newUser, command.ManagerDto.Password, CommonRoles.Manager, command.ManagerDto.RestaurantId));
        }
    }
}