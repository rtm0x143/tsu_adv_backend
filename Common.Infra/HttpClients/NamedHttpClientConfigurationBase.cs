namespace Common.Infra.HttpClients;

public abstract class NamedHttpClientConfigurationBase : INamedHttpClientConfiguration
{
    public string Name { get; }
    public Action<IServiceProvider, HttpClient> ConfigureClientMethod => ConfigureClient;

    protected abstract void ConfigureClient(IServiceProvider provider, HttpClient client);
    public NamedHttpClientConfigurationBase(string name) => Name = name;
}