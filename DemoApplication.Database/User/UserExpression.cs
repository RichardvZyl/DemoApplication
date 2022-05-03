using System.Linq.Expressions;
using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Database;

public static class UserExpression
{
    #region Values
    public static Expression<Func<User, Guid>> AuthId() => user => user.Auth!.Id;
    public static Expression<Func<User, Guid>> Id() => user => user.Id;
    public static Expression<Func<User, string>> Name() => user => user.FullName.Name;
    public static Expression<Func<User, string>> Surname() => user => user.FullName.Surname;
    public static Expression<Func<User, string>> FullName() => user => $"{user.FullName.Name} {user.FullName.Surname}";
    public static Expression<Func<User, string>> Email() => user => user.Email;
    public static Expression<Func<User, bool>> Active() => user => user.Active;
    public static Expression<Func<User, UserModel>> Model => user => new UserModel
    {
        Id = user.Id,
        Name = user.FullName.Name,
        Surname = user.FullName.Surname,
        Email = user.Email,
        Active = user.Active
    };
    #endregion

    #region Get By Values
    public static Expression<Func<User, bool>> Email(string email) => user => user.Email == email;
    public static Expression<Func<User, bool>> Id(Guid id) => user => user.Id == id;

    public static Expression<Func<User, bool>> AuthId(Guid id) => user => user.Auth!.Id == id;
    #endregion
}
