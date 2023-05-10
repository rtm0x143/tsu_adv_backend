using Auth.Features.Customer.Commands;
using Auth.Infra.Data.Entities;
using Auth.Infra.Data.IdentityServices;
using Auth.Converters;
using Common.App.Exceptions;
using Common.Domain.ValueTypes;
using Common.Infra.Auth;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.Controllers
{
    public partial class CustomerController
    {
        /// <summary>
        /// Registers new user as a customer
        /// </summary>
        /// <responce code="400">When some data were unsuitable</responce>
        /// <returns>Customer's id</returns>
        [HttpPost]
        public Task<ActionResult<IdResult>> Register(CustomerRegistrationDto dto,
            [FromServices] IRegisterCustomer useCase)
        {
            return useCase.Execute(new(dto))
                .ContinueWith<ActionResult<IdResult>>(task => task.Result.IsT0
                    ? Ok(task.Result.AsT0)
                    : BadRequest(task.Result.AsT1.ToProblemDetails()));
        }
    }
}

namespace Auth.Features.Customer.Commands
{
    public class RegisterCustomer : IRegisterCustomer
    {
        private readonly AuthUserManager _userManager;

        public RegisterCustomer(AuthUserManager userManager) => _userManager = userManager;

        public Task<OneOf<IdResult, UnsuitableDataException>> Execute(RegisterCustomerCommand command)
        {
            var newUser = command.CustomerRegistrationDto.AdaptToCustomer();
            return _userManager
                .CreateWithRolesAsync(newUser, command.CustomerRegistrationDto.Password, CommonRoles.Customer)
                .ContinueWith<OneOf<IdResult, UnsuitableDataException>>((t, user) =>
                {
                    if (t.Result.Succeeded) return new IdResult(((AppUser)user!).Id);
                    return UnsuitableDataException.FromIdentityResult(t.Result);
                }, newUser);
        }
    }
}