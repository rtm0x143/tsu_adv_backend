using System.Diagnostics.CodeAnalysis;
using OneOf;

namespace Notifications.Domain.ValueTypes;

public readonly record struct NotificationTopic
{
    public const string DirectTopicName = "direct";
    public const string BroadcastTopicName = "broadcast";
    public const char TopicStringSeparator = '/';

    public readonly string Name = BroadcastTopicName;
    public readonly string? NameIdentifier = null;

    private NotificationTopic(string name, string? identifier = null)
    {
        Name = name;
        NameIdentifier = identifier;
    }

    public override string ToString() => NameIdentifier == null ? Name : Name + TopicStringSeparator + NameIdentifier;

    public bool IsDirect([NotNullWhen(true)] out string? nameIdentifier)
    {
        nameIdentifier = this is { Name: DirectTopicName, NameIdentifier: { } }
            ? NameIdentifier
            : null;
        return nameIdentifier != null;
    }

    public bool IsBroadcast() => this is { Name: BroadcastTopicName, NameIdentifier: null };

    public static OneOf<NotificationTopic, ArgumentException> FromString(string @string)
    {
        var parts = @string.Split(TopicStringSeparator);
        if (parts.Length is < 1 or > 2)
            return new ArgumentException($"'{@string}' is incorrect formatted for notification topic string",
                nameof(@string));

        return NotificationTopic.Construct(parts[0], parts.Length == 2 ? parts[1] : null);
    }

    private static bool _validateString(string @string, [NotNullWhen(false)] out ArgumentException? exception,
        string? paramName = null)
    {
        exception = @string.Contains(TopicStringSeparator)
            ? new ArgumentException("Parts of notification topic can't contain '/' character", paramName)
            : null;
        exception = string.IsNullOrWhiteSpace(@string)
            ? new ArgumentException("Parts of notification topic can't be empty string or whitespace", paramName)
            : null;

        return exception == null;
    }

    public static OneOf<NotificationTopic, ArgumentException> Direct(string identifier)
    {
        if (!_validateString(identifier, out var exception, nameof(identifier))) return exception;
        return new NotificationTopic(DirectTopicName, identifier);
    }

    public static NotificationTopic Broadcast() => new(BroadcastTopicName);

    public static OneOf<NotificationTopic, ArgumentException> Construct(string name, string? identifier)
    {
        if (!_validateString(name, out var exception, nameof(identifier))) return exception;
        if (identifier != null && !_validateString(identifier, out exception, nameof(name))) return exception;
        return new NotificationTopic(name, identifier);
    }
}