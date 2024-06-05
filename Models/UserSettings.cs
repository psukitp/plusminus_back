﻿namespace plusminus.Models;

public class UserSettings
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Locale { get; set; } = "en";
    public string Theme { get; set; } = "light";
    public virtual User User { get; set; } = null!;
}