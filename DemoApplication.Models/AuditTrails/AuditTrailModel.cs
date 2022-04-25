using System;

namespace DemoApplication.Models;

/// <summary>
/// the audit trail entity definition
/// </summary>
public class AuditTrailModel
{
    /// <summary>
    /// the primary key of this entity
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// the date and time of this entry
    /// </summary>
    public DateTimeOffset Date { get; set; }

    /// <summary>
    /// the user id causing this audit trail entry
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// the display context of this entry - something nice to display on the screen
    /// </summary>
    public string DisplayContext { get; set; }

    /// <summary>
    /// the model used for serialized information
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// the serialized content
    /// </summary>
    public string Contents { get; set; }
}
