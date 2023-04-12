using Auth.Features.User.Commands;
using Auth.Infra.Data;
using Auth.Infra.Data.Entities;
using Auth.Mappers.Generated;
using Common.App.Exceptions;
using Common.App.Models.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.ControllersOrigin
{
    public partial class UserController
    {
        /// <summary>
        /// Registers new user
        /// </summary>
        /// <responce code="400">When some data were unsuitable</responce>
        /// <returns>Created user id</returns>
        [HttpPost]
        public Task<ActionResult<IdResult>> Register(RegisterUserCommand registerManagerCommand,
            [FromServices] RegisterUser useCase)
        {
            return useCase.Execute(registerManagerCommand)
                .ContinueWith<ActionResult<IdResult>>(t => t.Result.IsT0
                    ? Ok(t.Result.AsT0)
                    : BadRequest(t.Result.AsT1.ToProblemDetails()));
        }
    }
}

namespace Auth.Features.User.Commands
{
    public class RegisterUser : IRegisterUser
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthDbContext _context;

        public RegisterUser(UserManager<AppUser> userManager, AuthDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public Task<OneOf<IdResult, UnsuitableDataException>> Execute(RegisterUserCommand command)
        {
            var newUser = command.UserDto.AdaptToAppUser();
            return _userManager.CreateAsync(newUser, command.UserDto.Password)
                .ContinueWith<OneOf<IdResult, UnsuitableDataException>>((t, user) =>
                {
                    if (t.Result.Succeeded) return new IdResult(((AppUser)user!).Id);
                    return UnsuitableDataException.FromIdentityResult(t.Result);
                }, newUser);
        }
    }
}