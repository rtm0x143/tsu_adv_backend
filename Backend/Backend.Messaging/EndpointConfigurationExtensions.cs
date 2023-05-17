using Common.Infra.Messaging;
using NServiceBus;

namespace Backend.Messaging;

public static class EndpointConfigurationExtensions
{
    public static void AddBackendMessages(this EndpointConfiguration configuration)
        => configuration.Conventions().Add(new NamespaceMessageConvention
        {
            BaseNamespace = string.Join('.', nameof(Backend), nameof(Backend.Messaging)),
            MessagesSuffix = nameof(Backend.Messaging.Messages)
        });
}