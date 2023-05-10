using Microsoft.Extensions.Configuration;

namespace Common.App.Utils;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Extract string from <paramref name="configuration"/> or throws
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="keys">Keys to search for</param>
    /// <exception cref="ArgumentException">When value was not found</exception>
    public static string GetRequiredString(this IConfiguration configuration, params string[] keys)
    {
        if (keys.Select(configuration.GetValue<string>).OfType<string>().FirstOrDefault() is string value)
            return value;

        throw new ArgumentException("Required value was not found in configuration. " +
                                    $"Searched for keys : {string.Join(" ,", keys.Select(key => $"'{key}'"))}.");
    }
}