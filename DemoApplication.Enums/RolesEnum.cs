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
    {// TODO: should pull this from DB
        return (role == RolesEnum.Administrator
             || role == RolesEnum.Interface
             || role == RolesEnum.Supervisor)
        ? true : false;
    }
}

