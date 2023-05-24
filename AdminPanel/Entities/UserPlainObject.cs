using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Entities;

public class UserPlainObject
{
    [HiddenInput] public Guid Id { get; set; }
    public string Email { get; set; }
    public string Fullname { get; set; }
    public string PhoneNumber { get; set; }
    public string Gender { get; set; }
    public DateOnly? BirthDate { get; set; }
    public bool IsBanned { get; set; }
    [HiddenInput] public Guid? Restaurant { get; set; }
    [HiddenInput] public string Roles { get; set; }
    [HiddenInput] public string AllUserClaims { get; set; }
}

public class UserPlainObjectMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserPlainObject>()
            .Map(dest => dest.Roles, src => string.Join(Environment.NewLine, src.Roles))
            .Map(dest => dest.AllUserClaims,
                src => string.Join(Environment.NewLine,
                    src.AllUserClaims.Select(claim => $"{claim.Key} = {claim.Value}")))
            .GenerateMapper(MapType.Map);
    }
}