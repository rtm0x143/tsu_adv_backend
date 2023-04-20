namespace Common.Infra.Auth.Policies;

/// <summary>
/// If user satisfies requirement all other requirement will be omitted  
/// </summary>
public interface IOrAbsolutePrivilegeRequirement : ISelfProcessingRequirement
{
};