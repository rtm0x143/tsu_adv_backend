using System.Diagnostics.CodeAnalysis;

namespace Common.Infra.Auth.Policies;

public static class HasClaimPolicy
{
    private const string PolicyPrefix = "HasClaim";
    private const char ValidValuesSeparator = ',';
    private const char ClaimTypeValueSeparator = '|';

    private static void _throwIfInvalid(string @string)
    {
        if (@string.Contains(ValidValuesSeparator))
            throw new ArgumentException(
                $"Value {@string} contains '{ValidValuesSeparator}' symbols what is unacceptable");
        if (@string.Contains(ClaimTypeValueSeparator))
            throw new ArgumentException(
                $"Value {@string} contains '{ClaimTypeValueSeparator}' symbols what is unacceptable");
    }

    /// <param name="claimType"><inheritdoc cref="Name(string)"/></param>
    /// <param name="validValues">Can't contain <see cref="ValidValuesSeparator"/> or <see cref="ClaimTypeValueSeparator"/> symbols</param>
    public static string Name(string claimType, IEnumerable<string> validValues)
    {
        foreach (var value in validValues) _throwIfInvalid(value);
        return Name(claimType) + ClaimTypeValueSeparator + string.Join(ValidValuesSeparator, validValues);
    }

    /// <inheritdoc cref="Name(string,System.Collections.Generic.IEnumerable{string})"/>
    public static string Name(string claimType, params string[] validValues) =>
        Name(claimType, (IEnumerable<string>)validValues);

    /// <param name="claimType">Can't contain <see cref="ValidValuesSeparator"/> or <see cref="ClaimTypeValueSeparator"/> symbols</param>
    public static string Name(string claimType)
    {
        _throwIfInvalid(claimType);
        return PolicyPrefix + claimType;
    }

    public static bool TryParseName(string name, [NotNullWhen(true)] out string? claimType,
        out IEnumerable<string>? validValues)
    {
        claimType = null;
        validValues = null;
        if (!name.StartsWith(PolicyPrefix)) return false;

        var parts = name[PolicyPrefix.Length..].Split(ClaimTypeValueSeparator);
        if (parts.Length == 0) return false;
        claimType = parts[0];
        if (parts.Length > 1) validValues = parts[1].Split(ValidValuesSeparator);
        return true;
    }
}