using System.Reflection;
using Auth.Infra.Data.Configuration;
using Auth.Infra.Services;
using Backend.Messaging;
using Backend.Messaging.Messages.Events;
using Common.App.Configure;
using Common.App.RequestHandlers;
using Common.App.Utils;
using Common.Infra.Messaging;
using Common.Infra.Services.SMTP;
using NServiceBus;
using EndpointConfiguration = NServiceBus.EndpointConfiguration;

namespace Auth.Infra.Messaging;
public class AuthMessageBusConfiguration : MessageBusConfiguration
{
    protected override EndpointConfiguration CreateEndpointConfiguration(HostBuilderContext context)
    {
        const string endpointName = $"{nameof(Auth)}.Messaging";
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
        services.AddIdentityServices(context.Configuration);
        services.AddInfraServices(context.Configuration)
            .AddCommonSmtpServices(context.Configuration)
            .AddCommonAppServices()
            .AddRequestHandlersFrom(Assembly.GetExecutingAssembly());
    }
}