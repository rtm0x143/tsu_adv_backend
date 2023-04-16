using System.ComponentModel.DataAnnotations;
using Auth.Features.RestaurantAdmin.Commands;
using Auth.Features.User.Commands;
using Auth.Infra.Auth.Policies;
using Auth.Mappers.Generated;
using Common.App.Models.Results;
using Common.App.Utils;
using Common.Infra.Auth;
using Common.Infra.Auth.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.Controllers
{
    public partial class RestaurantAdminController
    {
        /// <summary>
        /// Registers new user as a RestaurantAdmin
        /// </summary>
        /// <responce code="400">When some data were unsuitable</responce>
        /// <responce code="403">When caller has not enough permissions</responce>
        /// <returns>New user's id</returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IdResult>> Register(RestaurantAdminRegistrationDto dto,
            [FromServices] IRegisterRestaurantAdmin useCase)
        {
            var authResult = await AuthService.AuthorizeAsync(User, null,
                new GrantInRestaurantRequirement(dto.RestaurantId, CommonRoles.RestaurantAdmin));
            if (!authResult.Succeeded) return Forbid();

            var result = await useCase.Execute(new RegisterRestaurantAdminCommand(dto));
            return result.IsT0 ? Ok(result.AsT0) : ExceptionsDescriber.Describe(result.Value);
        }
    }
}

namespace Auth.Features.Customer.Commands
{
    public class RegisterRestaurantAdmin : IRegisterRestaurantAdmin
    {
        private readonly IRegisterRestaurantRelatedUser _register;
        public RegisterRestaurantAdmin(IRegisterRestaurantRelatedUser register) => _register = register;

        public Task<OneOf<IdResult, Exception>> Execute(RegisterRestaurantAdminCommand command)
        {
            var newUser = new Infra.Data.Entities.RestaurantAdmin();
            command.RestaurantAdminDto.AdaptTo(newUser);

            return _register.Execute(
                new(newUser, command.RestaurantAdminDto.Password, CommonRoles.Cook, command.RestaurantAdminDto.RestaurantId));
        }
    }
}