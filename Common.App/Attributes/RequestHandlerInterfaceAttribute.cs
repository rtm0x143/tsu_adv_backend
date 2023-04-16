namespace Common.App.Attributes;

/// <summary>
/// Annotates interface as request handler, used to select them threw reflection
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public sealed class RequestHandlerInterfaceAttribute : Attribute
{
}