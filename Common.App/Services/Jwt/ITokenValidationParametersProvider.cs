using Microsoft.IdentityModel.Tokens;

namespace Common.App.Services.Jwt;

public interface ITokenValidationParametersProvider
{
    TokenValidationParameters ValidationParameters { get; }   
}