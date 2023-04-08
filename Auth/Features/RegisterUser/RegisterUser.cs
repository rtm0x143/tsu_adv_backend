using System.ComponentModel.DataAnnotations;
using Auth.Features.RegisterUser;
using Auth.Infra.Data.Entities;
using Auth.Mappers.Generated;
using Common.App.Dtos.Results;
using Common.App.Exceptions;
using Mapster;
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

namespace Auth.Features.RegisterUser
{
    public record RegisterUserCommand
    {
        public required string Fullname { get; set; }
        [EmailAddress] public required string Email { get; set; }
        public DateOnly? BirthDate { get; set; }
        public Gender Gender { get; set; }
        [Phone] public required string PhoneNumber { get; set; }
        public required string Password { get; set; }


        internal class MapperRegister : IRegister
        {
            public virtual void Register(TypeAdapterConfig config)
            {
                config.NewConfig<RegisterUserCommand, AppUser>()
                    .Map(d => d.UserName, s => s.Fullname)
                    .GenerateMapper(MapType.Map | MapType.MapToTarget);
            }
        }
    }

    public class RegisterUser : IRegisterUser
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterUser(UserManager<AppUser> userManager) => _userManager = userManager;

        public Task<OneOf<IdResult, UnsuitableDataException>> Execute(RegisterUserCommand command)
        {
            var newUser = command.AdaptToAppUser();
            return _userManager.CreateAsync(newUser, command.Password)
                .ContinueWith<OneOf<IdResult, UnsuitableDataException>>((t, user) =>
                {
                    if (t.Result.Succeeded) return new IdResult(((AppUser)user!).Id);
                    return UnsuitableDataException.FromIdentityResult(t.Result);
                }, newUser);
        }
    }
}