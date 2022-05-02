using Abstractions.Domain;
using DemoApplication.Enums;

namespace DemoApplication.Domain;

public class MakerChecker : Entity<Guid>
{
    #region Constructor
    public MakerChecker
    (
        MakerCheckerActionsEnum action,
        Guid makerUser,
        Guid checkerUser,
        DateTimeOffset makerDate,
        DateTimeOffset? checkerDate,
        bool accepted,
        string motivation,
        IEnumerable<Guid> files,
        string model
    )
    {
        Action = action;
        MakerUser = makerUser;
        CheckerUser = checkerUser;
        MakerDate = makerDate;
        CheckerDate = checkerDate;
        Accepted = accepted;
        Motivation = motivation;
        Files = files;
        Model = model;
    }
    public MakerChecker(Guid id) : base(id) { }
    #endregion

    #region Private Members
    public MakerCheckerActionsEnum Action { get; private set; }
    public Guid MakerUser { get; private set; }
    public Guid CheckerUser { get; private set; }
    public bool Accepted { get; set; }
    public DateTimeOffset MakerDate { get; private set; }
    public DateTimeOffset? CheckerDate { get; private set; }
    public string Motivation { get; private set; } = string.Empty;
    public IEnumerable<Guid> Files { get; private set; } = Array.Empty<Guid>();
    public string Model { get; private set; } = string.Empty;
    #endregion

    #region Interactions
    public void Approve(Guid checkerUser)
    {
        this.CheckerUser = checkerUser;
        this.CheckerDate = DateTimeOffset.UtcNow;
        this.Accepted = true;
    }
    public void Deny(Guid checkerUser)
    {
        this.CheckerUser = checkerUser;
        this.CheckerDate = DateTimeOffset.UtcNow;
        this.Accepted = false;
    }
    #endregion
}
