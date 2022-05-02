using Abstractions.Domain;

namespace DemoApplication.Domain;

public class User : Entity<Guid> //TODO: Implement IdentityUser<Guid> (AspNetCore Identity)
{
    #region Constructor
    public User
    (
        FullName fullName,
        string email,
        Auth auth
    )
    {
        FullName = fullName;
        Email = email;
        Auth = auth;
        Activate();
    }
    public User(Guid id) : base(id) { }
    #endregion

    #region Private Members
    // public override Guid Id => Auth?.Id ?? base.Id; //TODO: Implement this correctly Auth is not always included with user //But the Id's should always coincide

    public FullName FullName { get; private set; } = new(string.Empty, string.Empty);

    //TODO: Use Auth's Email save Email only once //keep in mind don't always want auth to be fetched with user // find a way to do this correctly
    public string Email { get; private set; } = string.Empty;
    public bool Active { get; private set; } = false;
    public Auth? Auth { get; private set; }
    #endregion

    #region Interactions
    public void Activate()
        => Active = true;
    //Status = UserStatusEnum.Active;

    public void Inactivate()
        => Active = false;
    //Status = UserStatusEnum.Inactive;

    public void ChangeFullName(string name, string surname)
        => FullName = new FullName(name, surname);

    public void ChangeEmail(string email)
        => Email = email;
    #endregion
}
