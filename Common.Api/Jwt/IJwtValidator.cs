﻿using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Common.Api.jwt;

public interface IJwtValidator
{
    bool ValidateToken(string token, [NotNullWhen(true)] out ClaimsPrincipal? claims, [NotNullWhen(false)] out Exception? problem);
}
