using System.Reflection;
using System.Text.Json;
using Backend.Infra.Data;
using Backend.Messaging;
using Backend.Messaging.Messages.Events;
using Common.App.Configure;
using Common.App.RequestHandlers;
using Common.App.Utils;
using Common.Infra.Messaging;
using NServiceBus;

namespace Backend.Infra.Messaging;

public class BackendMessageBusConfiguration : MessageBusConfiguration
{
    protected override EndpointConfiguration CreateEndpointConfiguration(HostBuilderContext context)
    {
        const string endpointName = $"{nameof(Backend)}.Messaging";
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Classic),
            context.Configuration.GetRequiredString("MQ_URI", "ConnectionStrings:MQUri"));

        endpointConfiguration.Conventions()
            .Add(BackendMessageConvention.Instance);
        endpointConfiguration.UseTransport(transport)
            .RouteToEndpoint(Assembly.GetAssembly(typeof(RestaurantCreatedEvent)), endpointName);
        
        return endpointConfiguration;
    }

    public override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddBackendDbContexts(context.Configuration)
            .AddCommonAppServices()
            .AddRequestHandlersFrom(Assembly.GetExecutingAssembly());
    }
}