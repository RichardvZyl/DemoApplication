using DemoApplication.Enums;

namespace DemoApplication.Models;

public class AuthModel
{
    public string Login { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public RolesEnum Role { get; set; }
}
