using Abstractions.Domain;

namespace DemoApplication.Domain;

public sealed class AuditTrail : Entity<Guid>
{
    #region Constructor
    public AuditTrail
    (
        DateTimeOffset date,
        Guid userId,
        string displayContext,
        string model,
        string contents
    )
    {
        Date = date;
        UserId = userId;
        DisplayContext = displayContext;
        Model = model;
        Contents = contents;
    }
    public AuditTrail(Guid id) : base(id) { }
    #endregion

    #region Private Members
    //public Guid Id { get; set; }
    public DateTimeOffset Date { get; private set; }
    public Guid UserId { get; private set; }
    public string DisplayContext { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public string Contents { get; private set; } = string.Empty;
    #endregion
}
