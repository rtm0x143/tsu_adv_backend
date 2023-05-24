using Common.Infra.Messaging;

namespace Backend.Messaging;

public static class BackendMessageConvention
{
    public static NamespaceMessageConvention Instance { get; } = new()
    {
        BaseNamespace = string.Join('.', nameof(Backend), nameof(Backend.Messaging)),
        MessagesSuffix = nameof(Backend.Messaging.Messages),
    };
}