using NServiceBus;

namespace Common.Infra.Messaging;

public class NamespaceMessageConvention : IMessageConvention
{
    public bool IsMessageType(Type type) => type.Namespace?.StartsWith(MessagesNamespace) ?? false;

    public bool IsCommandType(Type type) => type.Namespace?.StartsWith(CommandsNamespace) ?? false;

    public bool IsEventType(Type type) => type.Namespace?.StartsWith(EventsNamespace) ?? false;

    public string Name => "Namespace message convention";
    public string? MessagesSuffix { get; init; } = "Messages";
    public string EventsSuffix { get; init; } = "Events";
    public string CommandsSuffix { get; init; } = "Commands";

    public required string BaseNamespace;

    public string MessagesNamespace
        => MessagesSuffix == null ? BaseNamespace : string.Join('.', BaseNamespace, MessagesSuffix);

    public string EventsNamespace => string.Join('.', MessagesNamespace, EventsSuffix);
    public string CommandsNamespace => string.Join('.', MessagesNamespace, CommandsSuffix);
}