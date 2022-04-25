using System;
using DemoApplication.Enums;

namespace DemoApplication.Models;

/// <summary>
/// notification model for the database entity
/// </summary>
public class AngularNotificationModel
{
    public Guid Id { get; set; }
    public Guid Originator { get; set; }
    public string OriginatorString { get; set; }
    public SeverityEnum Severity { get; set; }
    public string Description { get; set; }
    public DateTimeOffset Date { get; set; }
    public bool Read { get; set; }
    public Guid SeenBy { get; set; }
    public string SeenByUser { get; set; }
    public DateTimeOffset? SeenAt { get; set; }
    public RolesEnum ForRole { get; set; }
    public string RelatedId { get; set; }
    public string RelatedDescription { get; set; }
    public EntityEnum Entity { get; set; }
}
