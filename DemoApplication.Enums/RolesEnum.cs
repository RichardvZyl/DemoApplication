namespace DemoApplication.Enums;

public enum RolesEnum
{
    Clerk = 1,
    Financial,
    Interface,
    Supervisor,
    Administrator
}

public class DecryptRoles
{
    public static bool ShouldDecrypt(RolesEnum role) 
        //TODO: Fetch from db should not be hardcoded
        => role is RolesEnum.Administrator
             or RolesEnum.Interface
             or RolesEnum.Supervisor;
}

