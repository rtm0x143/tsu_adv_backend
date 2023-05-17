using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.HttpClients;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Applies <see cref="CommonNamedHttpClientConfiguration"/> for each key in <paramref name="configurationSection"/>>
    /// and configures <see cref="ExternalApiOptions"/> for each it's value.
    /// Also uses <paramref name="configurations"/> to configure named <see cref="HttpClient"/>s 
    /// </summary>
    /// <param name="services">Target service collection</param>
    /// <param name="configurationSection">Configuration to reed from</param>
    /// <param name="configurations">Other <see cref="INamedHttpClientConfiguration"/>s to apply</param>
    public static IServiceCollection AddCommonHttpClientConfiguration(this IServiceCollection services,
        IConfigurationSection configurationSection,
        params INamedHttpClientConfiguration[] configurations)
    {
        services.Configure<CommonHttpClientConfigurationOptions>(
            options => options.ConfigurationSection = configurationSection);
        services.AddSingleton<IConfigureNamedOptions<ExternalApiOptions>, ConfigureExternalApiOptions>();
        services.AddSingleton<IConfigureOptions<ExternalApiOptions>, ConfigureExternalApiOptions>();

        foreach (var namedHttpClientConfiguration in configurations.Concat(
                     configurationSection.GetChildren()
                         .Select(child => new CommonNamedHttpClientConfiguration(child.Key))))
        {
            services.AddHttpClient(namedHttpClientConfiguration.Name)
                .ConfigureHttpClient(namedHttpClientConfiguration.ConfigureClientMethod);
        }

        return services;
    }

    public const string DefaultConfigurationSectionName = "ExternalApis";

    /// <summary>
    /// Calls <see cref="AddCommonHttpClientConfiguration(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Configuration.IConfigurationSection,Common.Infra.HttpClients.INamedHttpClientConfiguration[])"/>
    /// with <see cref="DefaultConfigurationSectionName"/>
    /// </summary>
    public static IServiceCollection AddCommonHttpClientConfiguration(this IServiceCollection services,
        IConfiguration configuration, params INamedHttpClientConfiguration[] configurations)
        => services.AddCommonHttpClientConfiguration(
            configuration.GetRequiredSection(DefaultConfigurationSectionName), configurations);
}