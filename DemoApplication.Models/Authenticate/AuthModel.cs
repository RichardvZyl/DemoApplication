using DemoApplication.Enums;

namespace DemoApplication.Models;

public class AuthModel
{
    public string Login { get; set; }

    public string Password { get; set; }

    public RolesEnum Role { get; set; }
}
