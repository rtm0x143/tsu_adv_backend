﻿using System.Text.Json.Serialization;

namespace Backend.Infra.Data.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DishCategory
{
    Wok,
    Pizza,
    Soup,
    Desert,
    Drink
}