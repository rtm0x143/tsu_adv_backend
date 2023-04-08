using Auth.Features.RegisterUser;
using Auth.Infra.Data;
using Auth.Infra.Data.Entities;
using Auth.Mappers.Generated;
using Common.App.Dtos.Results;
using Common.App.Exceptions;
using Common.Infra.Auth;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OneOf;

namespace Auth.ControllersOrigin
{
    public partial class CustomerController
    {
        /// <summary>
        /// Registers new customer user
        /// </summary>
        /// <responce code="400">When some data were unsuitable</responce>
        /// <returns>Customer's id</returns>
        [HttpPost]
        public Task<ActionResult<IdResult>> Register(RegisterCustomerCommand registerCustomerCommand,
            [FromServices] RegisterCustomer useCase)
        {
            return useCase.Execute(registerCustomerCommand)
                .ContinueWith<ActionResult<IdResult>>(task =>
                {
                    if (task.Result.IsT1) return BadRequest(task.Result.AsT1.ToProblemDetails());
                    return Ok(task.Result.Value);
                });
        }
    }
}

namespace Auth.Features.RegisterUser
{
    public record RegisterCustomerCommand : RegisterUserCommand
    {
        public required string Address { get; set; }

        internal new class MapperRegister : IRegister
        {
            public void Register(TypeAdapterConfig config)
            {
                config.NewConfig<RegisterCustomerCommand, CustomerData>()
                    .BeforeMapping((src, dest) => ((RegisterUserCommand)src).AdaptTo(dest.User))
                    .GenerateMapper(MapType.Map | MapType.MapToTarget);
            }
        }
    }

    public class RegisterCustomer : IRegisterCustomer
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRegisterUser _registerUser;
        private readonly AuthDbContext _dbContext;

        public RegisterCustomer(UserManager<AppUser> userManager, IRegisterUser registerUser, AuthDbContext dbContext)
        {
            _userManager = userManager;
            _registerUser = registerUser;
            _dbContext = dbContext;
        }

        public async Task<OneOf<IdResult, UnsuitableDataException>> Execute(RegisterCustomerCommand command)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            var result = await _registerUser.Execute(command);
            if (result.IsT1) return result.AsT1;
            var user = await _userManager.FindByIdAsync(result.AsT0.Id.ToString())
                       ?? throw new UnexpectedException($"Successfully created user({result.AsT0.Id} can't be found");

            var identityResult = await _userManager.AddToRoleAsync(user, nameof(RoleNames.Customer));
            if (!identityResult.Succeeded) return UnsuitableDataException.FromIdentityResult(identityResult);

            await _dbContext.CustomersData.AddAsync(new() { Address = command.Address, User = user });
            
            await transaction.CommitAsync();
            return new IdResult(user.Id);
        }
    }
}