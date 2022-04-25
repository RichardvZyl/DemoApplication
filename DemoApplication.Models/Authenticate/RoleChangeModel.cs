using DemoApplication.Enums;

namespace DemoApplication.Models;

public class RoleChangeModel
{
    public string Login { get; set; }
    public RolesEnum Role { get; set; }

    public EntitlementExceptionsModel ClaimExceptions { get; set; }
}
