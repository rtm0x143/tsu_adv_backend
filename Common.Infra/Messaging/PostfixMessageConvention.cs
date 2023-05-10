using NServiceBus;

namespace Common.Infra.Messaging;

public class PostfixMessageConvention : IMessageConvention
{
    public bool IsMessageType(Type type)
        => IsCommandType(type) || IsEventType(type) || type.Name.EndsWith("Message", false, null);

    public bool IsCommandType(Type type) => type.Name.EndsWith("Command", false, null);

    public bool IsEventType(Type type) => type.Name.EndsWith("Event", false, null);

    public string Name => "Postfix message convention";
}