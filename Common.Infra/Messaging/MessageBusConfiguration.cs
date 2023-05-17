using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Common.Infra.Messaging;

public abstract class MessageBusConfiguration
{
    protected abstract EndpointConfiguration CreateEndpointConfiguration(HostBuilderContext context);

    public EndpointConfiguration BuildEndpointConfiguration(HostBuilderContext context)
    {
        var configuration = CreateEndpointConfiguration(context);
        configuration.UseSerialization<NewtonsoftJsonSerializer>();
        configuration.EnableInstallers();
        return configuration;
    }

    public abstract void ConfigureServices(HostBuilderContext context, IServiceCollection services);
}