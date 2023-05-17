namespace Common.Infra.HttpClients;

public interface INamedHttpClientConfiguration
{
    string Name { get; }
    Action<IServiceProvider, HttpClient> ConfigureClientMethod { get; }
}