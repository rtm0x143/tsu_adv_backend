using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infra.Data.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
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

public class Customer : AppUser
{
    public string Address { get; set; } = default!;
}

public class Courier : AppUser
{
}

public class Admin : AppUser
{
}

public class RestaurantOwner : AppUser
{
}

public class RestaurantAdmin : AppUser
{
}

public class Manager : AppUser
{
}

public class Cook : AppUser
{
}