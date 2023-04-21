using Microsoft.EntityFrameworkCore;

namespace Backend.Infra.Data.Entities;

[Index(nameof(Name), IsUnique = true)]
public class Restaurant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}