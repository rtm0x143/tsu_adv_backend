using Auth.Features.Courier.Commands;
using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Auth.Converters;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Common.Infra.Auth;
using Common.Infra.Auth.Policies;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.Controllers
{
    public partial class CourierController
    {
        /// <summary>
        /// Registers new user as a Courier
        /// </summary>
        /// <responce code="400">When some data were unsuitable</responce>
        /// <responce code="403">When caller has not enough permissions</responce>
        /// <returns>New user's id</returns>
        [HttpPost]
        [AuthorizeGrantRole(CommonRoles.Courier)]
        public Task<ActionResult<IdResult>> Register(CourierRegistrationDto dto,
            [FromServices] IRegisterCourier useCase)
        {
            return useCase.Execute(new RegisterCourierCommand(dto))
                .ContinueWith<ActionResult<IdResult>>(t => t.Result.IsT0
                    ? Ok(t.Result.AsT0)
                    : ExceptionsDescriber.Describe(t.Result.Value));
        }
    }
}

namespace Auth.Features.Courier.Commands
{
    public class RegisterCourier : IRegisterCourier
    {
        private readonly AuthUserManager _userManager;
        public RegisterCourier(AuthUserManager userManager) => _userManager = userManager;

        public Task<OneOf<IdResult, Exception>> Execute(RegisterCourierCommand command)
        {
            var newUser = new Infra.Data.Entities.Courier();
            command.CourierDto.AdaptTo(newUser);
            
            return _userManager.CreateWithRolesAsync(newUser, command.CourierDto.Password, CommonRoles.Courier)
                .ContinueWith<OneOf<IdResult, Exception>>((t, user) =>
                        t.Result.Succeeded
                            ? new IdResult(((AppUser)user!).Id)
                            : UnsuitableDataException.FromIdentityResult(t.Result),
                    newUser);
        }
    }
}