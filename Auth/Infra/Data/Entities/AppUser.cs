using Microsoft.AspNetCore.Identity;

namespace Auth.Infra.Data.Entities;

public enum Gender
{
    Unspesified,
    Male,
    Female
}

public class AppUser : IdentityUser<Guid>
{
    public Gender Gender { get; set; }
    public DateOnly? BirthDate { get; set; }
    public HashSet<AppUserRole> Roles { get; set; } = new();
    
}
