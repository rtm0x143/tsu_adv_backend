using System.Reflection;
using Backend.Infra.Data;
using Backend.Messaging.Events;
using Common.App.Configure;
using Common.App.RequestHandlers;
using Common.App.Utils;
using Common.Infra.Messaging;
using NServiceBus;

namespace Backend.Infra.Messaging;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureMessageBus(this IHostBuilder builder)
    {
        return builder.UseNServiceBus(BuildEndpoints)
            .ConfigureServices(ConfigureServices);
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddBackendDbContexts(context.Configuration)
            .AddCommonAppServices()
            .AddRequestHandlersFrom(Assembly.GetExecutingAssembly());
    }

    private static EndpointConfiguration BuildEndpoints(HostBuilderContext context)
    {
        const string endpointName = $"{nameof(Backend)}.Messaging";

        var endpointConfiguration = new EndpointConfiguration(endpointName);

        endpointConfiguration.EnableInstallers();
        if (context.HostingEnvironment.IsDevelopment())
            endpointConfiguration.UsePersistence<LearningPersistence>();

        var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Classic),
            context.Configuration.GetRequiredString("MQ_URI", "ConnectionStrings:MQUri"));

        endpointConfiguration.Conventions()
            .Add(new NamespaceMessageConvention
            {
                BaseNamespace = string.Join('.', nameof(Backend), nameof(Backend.Messaging)),
                MessagesSuffix = null
            });

        endpointConfiguration.UseTransport(transport)
            .RouteToEndpoint(Assembly.GetAssembly(typeof(RestaurantCreatedEvent)), endpointName);

        return endpointConfiguration;
    }
}