using System.Linq.Expressions;
using DemoApplication.Domain;
using DemoApplication.Enums;

namespace DemoApplication.Database;

public static class AuthExpression
{
    public static Expression<Func<Auth, RolesEnum>> Role() => auth => auth.Role;
    public static Expression<Func<Auth, bool>> Login(string login) => auth => auth.Email == login.ToLower();
    public static Expression<Func<Auth, string>> Salt() => auth => auth.Salt;
}
