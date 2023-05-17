using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace Common.Infra.Messaging;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureMessageBus<TMessageBusConfiguration>(this IHostBuilder builder)
        where TMessageBusConfiguration : MessageBusConfiguration, new()
    {
        var busConfiguration = new TMessageBusConfiguration();
        return builder.UseNServiceBus(busConfiguration.BuildEndpointConfiguration)
            .ConfigureServices(busConfiguration.ConfigureServices);
    }
}