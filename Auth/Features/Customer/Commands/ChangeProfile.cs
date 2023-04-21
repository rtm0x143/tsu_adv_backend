using Auth.Infra.Data.IdentityServices;
using Auth.Converters;
using Common.App.Exceptions;
using OneOf;
using EmptyResult = Common.App.Models.Results.EmptyResult;
using CustomerEntity = Auth.Infra.Data.Entities.Customer;

namespace Auth.Features.Customer.Commands
{
    public class ChangeProfile : IChangeProfile
    {
        private readonly AuthUserManager _userManager;
        public ChangeProfile(AuthUserManager userManager) => _userManager = userManager;

        public async Task<OneOf<EmptyResult, Exception>> Execute(ChangeProfileCommand command)
        {
            if (await _userManager.FindByIdAsync(command.CustomerId.ToString()) is not CustomerEntity customer)
                return new KeyNotFoundException(nameof(command.CustomerId));

            command.ProfileDto.AdaptTo(customer); 

            var result = await _userManager.UpdateAsync(customer);
            if (!result.Succeeded) return UnsuitableDataException.FromIdentityResult(result);
            return new EmptyResult();
        }
    }
}