using System.Reflection;
using Backend.Messaging;
using Backend.Messaging.Messages.Events;
using Common.App.RequestHandlers;
using Common.App.Utils;
using Common.Infra.Messaging;
using NServiceBus;

namespace Notifications.Infra;

public class NotificationsMessageBusConfiguration : MessageBusConfiguration
{
    protected override EndpointConfiguration CreateEndpointConfiguration(HostBuilderContext context)
    {
        const string endpointName = $"{nameof(Notifications)}.Messaging";
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Classic),
            context.Configuration.GetRequiredString("MQ_URI", "ConnectionStrings:MQUri"));

        endpointConfiguration.UseTransport(transport)
            .RouteToEndpoint(Assembly.GetAssembly(typeof(RestaurantCreatedEvent)), endpointName);

        endpointConfiguration.AddBackendMessages();
        return endpointConfiguration;
    }

    public override void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddRequestHandlersFrom(Assembly.GetExecutingAssembly())
            .AddDbContext<NotificationsDbContext>();
    }
}