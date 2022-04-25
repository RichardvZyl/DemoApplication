using System;

namespace DemoApplication.Models;

/// <summary>
/// user model for the database entity   
/// </summary>
public class UserModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public bool Active { get; set; }

    public AuthModel Auth { get; set; }
}
