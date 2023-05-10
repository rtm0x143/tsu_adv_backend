using Microsoft.IdentityModel.Tokens;

namespace Common.App.Services;

public interface ITokenValidationParametersProvider
{
    TokenValidationParameters ValidationParameters { get; }   
}