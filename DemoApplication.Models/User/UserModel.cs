using System;

namespace DemoApplication.Models;

public class UserModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public bool Active { get; set; }

    public AuthModel Auth { get; set; } = new();
}
