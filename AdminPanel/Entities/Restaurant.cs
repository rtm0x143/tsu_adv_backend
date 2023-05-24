using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Entities;

public class Restaurant
{
    public Restaurant()
    {
    }

    public Restaurant(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    [HiddenInput] public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
}