﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Common.Infra.HttpClients;

public class ExternalApiOptions
{
    public Uri Url { get; set; } = default!;
    public string? ApiKey { get; set; }
    public string? ApiBearer { get; set; }
}

public class CommonHttpClientConfigurationOptions
{
    public IConfigurationSection ConfigurationSection { get; set; } = default!;
}

internal class ConfigureExternalApiOptions : IConfigureNamedOptions<ExternalApiOptions>
{
    private readonly IOptionsMonitor<CommonHttpClientConfigurationOptions> _options;

    public ConfigureExternalApiOptions(IOptionsMonitor<CommonHttpClientConfigurationOptions> options) 
        => _options = options;

    public void Configure(ExternalApiOptions options) => Configure(Options.DefaultName, options);

    public void Configure(string? name, ExternalApiOptions options)
    {
        name ??= Options.DefaultName;
        _options.CurrentValue.ConfigurationSection.GetSection(name).Bind(options);
    }
}