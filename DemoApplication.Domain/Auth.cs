using DemoApplication.Enums;
using Abstractions.Domain;
using System;

namespace DemoApplication.Domain;

public sealed class Auth : Entity<Guid>
{
    #region Constructor
    public Auth
    (
        string email,
        string password,
        RolesEnum role
    )
    {
        Email = email;
        Password = password;
        Role = role;
        Salt = Guid.NewGuid().ToString();
    }

    public Auth(Guid id) : base(id) { }
    #endregion

    #region Private Members
    public string Email { get; set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string Salt { get; private set; } = string.Empty;
    public RolesEnum Role { get; private set; }
    #endregion

    #region Interactions
    //Email cannot be changed
    public void ChangePassword(string password) 
        => Password = password;
    public void ChangeRole(RolesEnum role) 
        => Role = role;
    public void ChangeLogin(string login) 
        => Email = login;
    #endregion
}
