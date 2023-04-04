namespace Auth.Infra.Data.Entities;

public class Customer : AppUser
{
    public required string Address { get; set; }
}