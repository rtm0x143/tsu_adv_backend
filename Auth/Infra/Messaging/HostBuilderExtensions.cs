using System.Reflection;
using Auth.Infra.Data.Configuration;
using Auth.Infra.Services;
using Common.App.Configure;
using Common.App.RequestHandlers;
using Common.App.Utils;
using Common.Infra.Messaging;
using Common.Infra.Services.SMTP;
using NServiceBus;
using EndpointConfiguration = NServiceBus.EndpointConfiguration;

namespace Auth.Infra.Messaging;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureMessageBus(this IHostBuilder builder)
    {
        return builder.UseNServiceBus(BuildEndpoints)
            .ConfigureServices(ConfigureServices);
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        services.AddIdentityServices(context.Configuration);
        services.AddInfraServices(context.Configuration)
            .AddCommonSmtpServices(context.Configuration)
            .AddCommonAppServices()
            .AddRequestHandlersFrom(Assembly.GetExecutingAssembly());
    }

    private static EndpointConfiguration BuildEndpoints(HostBuilderContext context)
    {
        const string endpointName = $"{nameof(Auth)}.Messaging";

        var endpointConfiguration = new EndpointConfiguration(endpointName);
        
        endpointConfiguration.EnableInstallers();
        if (context.HostingEnvironment.IsDevelopment())
            endpointConfiguration.UsePersistence<LearningPersistence>();

        var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Classic),
            context.Configuration.GetRequiredString("MQ_URI", "ConnectionStrings:MQUri"));

        endpointConfiguration.UseTransport(transport);

        return endpointConfiguration;
    }
}