using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Infra.HttpClients;

public static class ServiceCollectionExtensions
{
    ///<inheritdoc cref="AddCommonHttpClientConfiguration(IServiceCollection,IEnumerable{INamedHttpClientConfiguration})"/>
    public static IServiceCollection AddCommonHttpClientConfiguration(
        this IServiceCollection services,
        params INamedHttpClientConfiguration[] clientConfigurations)
        => services.AddCommonHttpClientConfiguration(clientConfigurations.AsEnumerable());

    /// <summary>
    /// Applies <paramref name="clientConfigurations"/> to configure named <see cref="HttpClient"/>s
    /// and configures <see cref="ExternalApiOptions"/> for each it's value.
    /// </summary>
    /// <param name="services">Target service collection</param>
    /// <param name="clientConfigurations">Other <see cref="INamedHttpClientConfiguration"/>s to apply</param>
    public static IServiceCollection AddCommonHttpClientConfiguration(
        this IServiceCollection services,
        IEnumerable<INamedHttpClientConfiguration> clientConfigurations)
    {
        services.AddSingleton<IConfigureNamedOptions<ExternalApiOptions>, ConfigureExternalApiOptions>();
        services.AddSingleton<IConfigureOptions<ExternalApiOptions>, ConfigureExternalApiOptions>();

        foreach (var namedHttpClientConfiguration in clientConfigurations)
        {
            services.AddHttpClient(namedHttpClientConfiguration.Name)
                .ConfigureHttpClient(namedHttpClientConfiguration.ConfigureClientMethod);
        }

        return services;
    }

    /// <summary>
    /// Applies <see cref="CommonNamedHttpClientConfiguration"/> for each key in <paramref name="configurationSection"/>>
    /// and configures <see cref="ExternalApiOptions"/> for each it's value.
    /// Also uses <paramref name="clientConfigurations"/> to configure named <see cref="HttpClient"/>s 
    /// </summary>
    /// <param name="services">Target service collection</param>
    /// <param name="configurationSection">Configuration to reed from</param>
    /// <param name="clientConfigurations">Other <see cref="INamedHttpClientConfiguration"/>s to apply</param>
    public static IServiceCollection AddCommonHttpClientConfiguration(
        this IServiceCollection services,
        IConfigurationSection configurationSection,
        params INamedHttpClientConfiguration[] clientConfigurations)
    {
        services.Configure<CommonHttpClientConfigurationOptions>(
            options => options.ConfigurationSection = configurationSection);

        foreach (var section in configurationSection.GetChildren())
        {
            services.Configure<ExternalApiOptions>(section.Key, section);
        }

        return services.AddCommonHttpClientConfiguration(
            clientConfigurations.Concat(
                configurationSection
                    .GetChildren()
                    .Select(child => new CommonNamedHttpClientConfiguration(child.Key))));
    }

    public const string DefaultConfigurationSectionName = "ExternalApis";

    /// <summary>
    /// Calls <see cref="AddCommonHttpClientConfiguration(IServiceCollection,IConfiguration,INamedHttpClientConfiguration[])"/>
    /// with <see cref="DefaultConfigurationSectionName"/>
    /// </summary>
    public static IServiceCollection AddCommonHttpClientConfiguration(this IServiceCollection services,
        IConfiguration configuration, params INamedHttpClientConfiguration[] clientConfigurations)
        => services.AddCommonHttpClientConfiguration(
            configuration.GetRequiredSection(DefaultConfigurationSectionName), clientConfigurations);
}