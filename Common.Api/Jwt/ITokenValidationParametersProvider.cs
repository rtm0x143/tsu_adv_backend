using Microsoft.IdentityModel.Tokens;

namespace Common.Api.jwt;

public interface ITokenValidationParametersProvider
{
    TokenValidationParameters ValidationParameters { get; }   
}