namespace Common.Infra.Auth;

public static class CommonClaimTypes
{
    /// <summary>
    /// Values are <see cref="CommonRoles"/>, defines which type of employee user can employ to restaurant   
    /// </summary>
    public const string Grant = "Grant";

    /// <summary>
    /// Value is Restaurant's id related to this user
    /// </summary>
    public const string Restaurant = "Restaurant";

    /// <summary>
    /// Indicates what can user do with personal data of other users
    /// Values are <see cref="CommonActionType"/> 
    /// </summary>
    public const string PersonalData = "PersonalData";

    /// <summary>
    /// Values are <see cref="CommonManageTargets"/>, indicates what targets can manage user
    /// </summary>
    public const string Manage = "Manage";
}