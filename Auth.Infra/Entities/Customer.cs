using Auth.Infra.Entities;

namespace Auth.Api.Data;

public class Customer : AppUser
{
    public string Address { get; set; }
}